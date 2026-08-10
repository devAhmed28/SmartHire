using FluentValidation;

namespace SmartHire.Application.Features.Admin.Commands.VerifyCompany
{
    public class VerifyCompanyCommandValidator : AbstractValidator<VerifyCompanyCommand>
    {
        public VerifyCompanyCommandValidator()
        {
            RuleFor(x => x.CompanyId)
                .NotEmpty().WithMessage("Company ID is required");
        }
    }
}
