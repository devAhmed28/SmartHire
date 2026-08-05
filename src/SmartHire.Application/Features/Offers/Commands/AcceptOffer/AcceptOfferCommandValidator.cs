using FluentValidation;

namespace SmartHire.Application.Features.Offers.Commands.AcceptOffer
{
    public class AcceptOfferCommandValidator : AbstractValidator<AcceptOfferCommand>
    {
        public AcceptOfferCommandValidator()
        {
            RuleFor(x => x.OfferId)
                .NotEmpty().WithMessage("Offer ID is required");

            RuleFor(x => x.CandidateId)
                .NotEmpty().WithMessage("Candidate ID is required");
        }
    }
}
