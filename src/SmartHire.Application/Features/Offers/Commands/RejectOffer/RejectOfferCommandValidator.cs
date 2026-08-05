using FluentValidation;

namespace SmartHire.Application.Features.Offers.Commands.RejectOffer
{
    public class RejectOfferCommandValidator : AbstractValidator<RejectOfferCommand>
    {
        public RejectOfferCommandValidator()
        {
            RuleFor(x => x.OfferId)
                .NotEmpty().WithMessage("Offer ID is required");

            RuleFor(x => x.CandidateId)
                .NotEmpty().WithMessage("Candidate ID is required");
        }
    }
}
