using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Admin;

namespace SmartHire.Application.Features.Admin.Commands.VerifyCompany
{
    public class VerifyCompanyCommandHandler : IRequestHandler<VerifyCompanyCommand, Result<CompanyAdminResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public VerifyCompanyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CompanyAdminResponse>> Handle(VerifyCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.Companies.GetByIdAsync(request.CompanyId, cancellationToken);

            if (company == null)
            {
                return Error.NotFound("Company");
            }

            if (company.IsVerified)
            {
                return Error.Conflict("Company is already verified");
            }

            company.Verify();

            var user = await _unitOfWork.Users.GetByIdAsync(company.UserId, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

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
