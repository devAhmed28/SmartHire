using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Offers;
using SmartHire.Domain.Entities;
using SmartHire.Domain.Enums;

namespace SmartHire.Application.Features.Offers.Commands.SendOffer
{
    public class SendOfferCommandHandler : IRequestHandler<SendOfferCommand, Result<OfferResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SendOfferCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<OfferResponse>> Handle(SendOfferCommand request, CancellationToken cancellationToken)
        {
            var application = await _unitOfWork.JobApplications.GetByIdAsync(request.ApplicationId,  cancellationToken);

            if (application == null)
            {
                return Error.NotFound("Application");
            }

            var job = await _unitOfWork.Jobs.GetByIdAsync(application.JobId, cancellationToken);

            if (job == null)
            {
                return Error.NotFound("Job");
            }

            if (job.CompanyId != request.CompanyId)
            {
                return Error.Forbidden("You do not have permission to send offers for this application");
            }

            var existingOffer = await _unitOfWork.Offers.GetByApplicationIdAsync(request.ApplicationId, cancellationToken);

            if (existingOffer != null)
            {
                return Error.Conflict("An offer has already been sent for this application");
            }

            var offer = new Offer(
                request.ApplicationId,
                request.Salary,
                request.Currency,
                request.StartDate,
                request.ExpirationDate,
                request.Notes
            );

            await _unitOfWork.Offers.AddAsync(offer, cancellationToken);

            application.UpdateStatus(ApplicationStatus.OfferSent);

            await _unitOfWork.SaveChangesAsync(cancellationToken); 

            var candidate = await _unitOfWork.CandidateProfiles.GetByIdAsync(application.CandidateProfileId, cancellationToken);

            var user = candidate != null ? await _unitOfWork.Users.GetByIdAsync(candidate.UserId, cancellationToken) : null;

            var company = await _unitOfWork.Companies.GetByIdAsync(job.CompanyId, cancellationToken);

            var response = new OfferResponse
            {
                Id = offer.Id,
                ApplicationId = offer.JobApplicationId,
                JobTitle = job.Title,
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
