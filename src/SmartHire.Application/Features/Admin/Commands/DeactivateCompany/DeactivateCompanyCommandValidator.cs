using FluentValidation;

namespace SmartHire.Application.Features.Admin.Commands.DeactivateCompany
{
    public class DeactivateCompanyCommandValidator : AbstractValidator<DeactivateCompanyCommand>
    {
        public DeactivateCompanyCommandValidator()
        {
            RuleFor(x => x.CompanyId)
                .NotEmpty().WithMessage("Company ID is required");
        }
    }
}
