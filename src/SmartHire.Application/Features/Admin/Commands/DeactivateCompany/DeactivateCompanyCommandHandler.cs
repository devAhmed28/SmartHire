using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Admin;

namespace SmartHire.Application.Features.Admin.Commands.DeactivateCompany
{
    public class DeactivateCompanyCommandHandler : IRequestHandler<DeactivateCompanyCommand, Result<CompanyAdminResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateCompanyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CompanyAdminResponse>> Handle(DeactivateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.Companies.GetByIdAsync(request.CompanyId, cancellationToken);

            if (company  == null)
            {
                return Error.NotFound("Company");
            }

            var user = await _unitOfWork.Users.GetByIdAsync(company.UserId, cancellationToken);

            if (user == null)
            {
                return Error.NotFound("User");
            }

            if (!user.IsActive)
            {
                return Error.Conflict("Company is already deactivated");
            }

            user.Deactivate();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new CompanyAdminResponse
            {
                Id = company.Id,
                CompanyName = company.CompanyName,
                Industry = company.Industry,
                Email = user.Email,
                IsVerified = company.IsVerified,
                IsActive = user.IsActive,
                JobCount = company.Jobs?.Count ?? 0,
                CreatedAt = company.CreatedAt
            };

            return Result.Success(response);
        }
    }
}
