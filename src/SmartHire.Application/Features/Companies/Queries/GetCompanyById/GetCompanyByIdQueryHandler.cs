using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Companies;

namespace SmartHire.Application.Features.Companies.Queries.GetCompanyById
{
    // to get a company by it's ID.
    public class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, Result<CompanyProfileResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCompanyByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CompanyProfileResponse>> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.Companies.GetCompanyWithDetailsAsync(request.CompanyId, cancellationToken);

            if (company == null)
            {
                return Error.NotFound("Company");
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
