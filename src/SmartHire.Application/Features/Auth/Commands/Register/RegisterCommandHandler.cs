using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Auth;
using SmartHire.Domain.Entities;
using SmartHire.Domain.Enums;

namespace SmartHire.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AuthResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public RegisterCommandHandler(
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            ITokenService tokenService,
            IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher =passwordHasher;
            _tokenService = tokenService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result<AuthResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var isCompany = request.AccountType == AccountType.Company;
            var role = isCompany ? UserRole.Company : UserRole.Candidate;

            if (request.AccountType != AccountType.Candidate && request.AccountType != AccountType.Company)
            {
                return Error.Validation("AccountType must be 'Candidate' or 'Company'");
            }

            if (isCompany && request.Company == null)
            {
                return Error.Validation("Company details are required for company registration");
            }

            if (!isCompany && request.Candidate == null)
            {
                return Error.Validation("Candidate details are required for candidate registration");
            }

            var isEmailUnique = await _unitOfWork.Users.IsEmailUniqueAsync(request.Email, cancellationToken);
            if (!isEmailUnique)
            {
                return Error.Conflict($"Email '{request.Email}' is already registered");
            }

            var isPhoneUnique = await _unitOfWork.Users.IsPhoneNumberUniqueAsync(request.PhoneNumber, cancellationToken);
            if (!isPhoneUnique)
            {
                return Error.Conflict($"Phone number '{request.PhoneNumber}' is already registered");
            }

            var hashedPassword = _passwordHasher.HashPassword(request.Password);

            var firstName = isCompany ? "Company" : request.Candidate!.FirstName;
            var lastName = isCompany ? "User" : request.Candidate!.LastName;

            var user = new User(
                firstName,
                lastName,
                request.Email,
                hashedPassword,
                request.PhoneNumber,
                role
            );

            await _unitOfWork.Users.AddAsync(user, cancellationToken);

            if (isCompany)
            {
                var companySize = MapCompanySize(request.Company!.CompanySize.ToString());

                var company = new Company(
                    user.Id,
                    request.Company.CompanyName,
                    request.Company.Description,
                    request.Company.Industry,
                    companySize
                );

                await _unitOfWork.Companies.AddAsync(company, cancellationToken);
            }
            else
            {
                var candidateProfile = new CandidateProfile(
                    user.Id,
                    bio: string.Empty,
                    currentPosition: string.Empty,
                    currentLocation: string.Empty,
                    yearsOfExperience: 0,
                    expectedSalary: 0
                );

                await _unitOfWork.CandidateProfiles.AddAsync(candidateProfile, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken(
                user.Id,
                refreshToken,
                _dateTimeProvider.UtcNow.AddDays(7)
            );
            await _unitOfWork.RefreshTokens.AddAsync(refreshTokenEntity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new AuthResponse
            {
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role.ToString(),
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = 900
            };

            return Result.Success(response);
        }

        private CompanySize MapCompanySize(string companySize)
        {
            return companySize?.ToLower() switch
            {
                "startup" => CompanySize.Startup,
                "small" => CompanySize.Small,
                "medium" => CompanySize.Medium,
                "large" => CompanySize.Large,
                "enterprise" => CompanySize.Enterprise,
                _ => CompanySize.Small
            };
        }
    }
}
