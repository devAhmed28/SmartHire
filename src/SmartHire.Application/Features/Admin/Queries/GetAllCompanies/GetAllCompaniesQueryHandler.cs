using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Admin;

namespace SmartHire.Application.Features.Admin.Queries.GetAllCompanies
{
    public class GetAllCompaniesQueryHandler : IRequestHandler<GetAllCompaniesQuery, Result<List<CompanyAdminResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllCompaniesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<CompanyAdminResponse>>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
        {
            var companies = await _unitOfWork.Companies.GetAllAsync(cancellationToken);

            var response = new List<CompanyAdminResponse>();

            foreach (var company in companies)
            {
                var user = await _unitOfWork.Users.GetByIdAsync(company.UserId, cancellationToken);

                var jobCount = company.Jobs?.Count ?? 0;

                response.Add(new CompanyAdminResponse
                {
                    Id = company.Id,
                    CompanyName = company.CompanyName,
                    Industry = company.Industry,
                    Email = user?.Email ?? string.Empty,
                    IsVerified = company.IsVerified,
                    IsActive = user?.IsActive ?? false,
                    JobCount = jobCount,
                    CreatedAt = company.CreatedAt
                });
            }

            return Result.Success(response);
        } 
    }
}
