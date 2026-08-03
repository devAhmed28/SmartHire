using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Companies;

namespace SmartHire.Application.Features.Companies.Queries.GetMyCompany
{
    // to get the current user's company profile.
    public class GetMyCompanyQueryHandler : IRequestHandler<GetMyCompanyQuery, Result<CompanyProfileResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMyCompanyQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CompanyProfileResponse>> Handle(GetMyCompanyQuery request, CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.Companies.GetByUserIdAsync(request.UserId, cancellationToken);

            if (company == null)
            {
                return Error.NotFound("Company not found for this user");
            }

            var user = await _unitOfWork.Users.GetByIdAsync(company.UserId, cancellationToken);

            if (user == null)
            {
                return Error.NotFound("User");
            }

            var response = new CompanyProfileResponse
            {
                Id = company.Id,
                UserId = company.UserId,
                CompanyName = company.CompanyName,
                Description = company.Description,
                Industry = company.Industry,
                WebsiteUrl = company.WebsiteUrl,
                LogoUrl = company.LogoUrl,
                LinkedInUrl = company.LinkedInUrl,
                Address = company.Address,
                City = company.City,
                Country = company.Country,
                FoundedYear = company.FoundedYear,
                IsVerified = company.IsVerified,
                CompanySize = company.CompanySize.ToString(),
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return Result.Success(response);
        }
    }
}