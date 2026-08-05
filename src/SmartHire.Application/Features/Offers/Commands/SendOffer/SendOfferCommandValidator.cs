using FluentValidation;

namespace SmartHire.Application.Features.Offers.Commands.SendOffer
{
    public class SendOfferCommandValidator : AbstractValidator<SendOfferCommand>
    {
        public SendOfferCommandValidator()
        {
            RuleFor(x => x.CompanyId)
                .NotEmpty().WithMessage("Company ID is required");

            RuleFor(x => x.ApplicationId)
                .NotEmpty().WithMessage("Application ID is required");

            RuleFor(x => x.Salary)
                .GreaterThan(0).WithMessage("Salary must be greater than 0");

            RuleFor(x => x.Currency)
                .IsInEnum().WithMessage("Invalid currency");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required")
                .GreaterThan(DateTime.UtcNow).WithMessage("Start date must be in the future");

            RuleFor(x => x.ExpirationDate)
                .NotEmpty().WithMessage("Expiration date is required")
                .GreaterThan(DateTime.UtcNow).WithMessage("Expiration date must be in the future")
                .GreaterThan(x => x.StartDate).WithMessage("Expiration date must be after start date");

            RuleFor(x => x.Notes)
                .MaximumLength(2000).WithMessage("Notes must not exceed 2000 characters");
        }
    }
}
