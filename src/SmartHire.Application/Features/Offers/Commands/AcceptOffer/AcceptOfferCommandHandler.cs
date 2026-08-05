using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Offers;
using SmartHire.Domain.Enums;

namespace SmartHire.Application.Features.Offers.Commands.AcceptOffer
{
    public class AcceptOfferCommandHandler : IRequestHandler<AcceptOfferCommand, Result<OfferResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AcceptOfferCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<OfferResponse>> Handle(AcceptOfferCommand request, CancellationToken cancellationToken)
        {
            var offer = await _unitOfWork.Offers.GetByIdAsync(request.OfferId, cancellationToken);

            if (offer == null)
            {
                return Error.NotFound("Offer");
            }

            var application = await _unitOfWork.JobApplications.GetByIdAsync(offer.JobApplicationId, cancellationToken);

            if (application == null)
            {
                return Error.NotFound("Application");
            }

            var candidate = await _unitOfWork.CandidateProfiles.GetByIdAsync(application.CandidateProfileId, cancellationToken);

            if (candidate == null || candidate.UserId != request.CandidateId)
            {
                return Error.Forbidden("You do not have permission to accept this offer");
            }

            if (offer.IsExpired())
            {
                return Error.Validation("This offer has expired");
            }

            if (offer.IsAccepted)
            {
                return Error.Conflict("This offer has already been accepted");
            }

            offer.Accept();

            application.UpdateStatus(ApplicationStatus.Accepted);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var job = await _unitOfWork.Jobs.GetByIdAsync(application.JobId, cancellationToken);

            var company = job != null ? await _unitOfWork.Companies.GetByIdAsync(job.CompanyId, cancellationToken) : null;

            var user = await _unitOfWork.Users.GetByIdAsync(candidate.UserId, cancellationToken);

            var response = new OfferResponse
            {
                Id = offer.Id,
                ApplicationId = offer.JobApplicationId,
                JobTitle = job?.Title ?? string.Empty,
                CompanyName = company?.CompanyName ?? string.Empty,
                CandidateName = user != null ? $"{user.FirstName} {user.LastName}" : string.Empty,
                CandidateEmail = user?.Email ?? string.Empty,
                Salary = offer.Salary,
                Currency = offer.Currency,
                StartDate = offer.StartDate,
                ExpirationDate = offer.ExpirationDate,
                IsAccepted = offer.IsAccepted,
                Notes = offer.Notes,
                CreatedAt = offer.CreatedAt,
                UpdatedAt = offer.UpdatedAt
            };

            return Result.Success(response);
        }
    }
}
