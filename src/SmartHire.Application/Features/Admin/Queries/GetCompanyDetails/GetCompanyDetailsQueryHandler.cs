using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Admin;

namespace SmartHire.Application.Features.Admin.Queries.GetCompanyDetails
{
    public class GetCompanyDetailsQueryHandler : IRequestHandler<GetCompanyDetailsQuery, Result<CompanyAdminResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCompanyDetailsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CompanyAdminResponse>> Handle(GetCompanyDetailsQuery request, CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.Companies.GetByIdAsync(request.CompanyId, cancellationToken);

            if (company == null)
            {
                return Error.NotFound("Company");
            }

            var user = await _unitOfWork.Users.GetByIdAsync(company.UserId, cancellationToken);

            var response = new CompanyAdminResponse
            {
                Id = company.Id,
                CompanyName = company.CompanyName,
                Industry = company.Industry,
                Email = user?.Email ?? string.Empty,
                IsVerified = company.IsVerified,
                IsActive = user?.IsActive ?? false,
                JobCount = company.Jobs?.Count ?? 0,
                CreatedAt = company.CreatedAt
            };

            return Result.Success(response);
        }
    }
}
