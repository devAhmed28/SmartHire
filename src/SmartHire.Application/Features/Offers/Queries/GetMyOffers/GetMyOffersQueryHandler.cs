using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Offers;

namespace SmartHire.Application.Features.Offers.Queries.GetMyOffers
{
    public class GetMyOffersQueryHandler : IRequestHandler<GetMyOffersQuery, Result<List<OfferResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMyOffersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<OfferResponse>>> Handle(GetMyOffersQuery request, CancellationToken cancellationToken)
        {
            var candidate = await _unitOfWork.CandidateProfiles.GetByIdAsync(request.CandidateId, cancellationToken);

            if (candidate == null)
            {
                return Error.NotFound("Candidate profile");
            }

            var offers = await _unitOfWork.Offers.GetByCandidateIdAsync(candidate.Id, cancellationToken);

            var response = new List<OfferResponse>();

            foreach (var offer in offers)
            {
                var application = await _unitOfWork.JobApplications.GetByIdAsync(offer.JobApplicationId, cancellationToken);

                var job = application != null ? await _unitOfWork.Jobs.GetByIdAsync(application.JobId, cancellationToken) : null;

                var company = job != null ? await _unitOfWork.Companies.GetByIdAsync(job.CompanyId, cancellationToken) : null;

                var user = await _unitOfWork.Users.GetByIdAsync(candidate.UserId, cancellationToken);

                response.Add(new OfferResponse
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
                });
            }

            return Result.Success(response);
        }
    }
}
