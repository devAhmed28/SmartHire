using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Offers;

namespace SmartHire.Application.Features.Offers.Queries.GetCompanyOffers
{
    public class GetCompanyOffersQueryHandler : IRequestHandler<GetCompanyOffersQuery, Result<List<OfferResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCompanyOffersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<OfferResponse>>> Handle(GetCompanyOffersQuery request, CancellationToken cancellationToken)
        {
            var offers = await _unitOfWork.Offers.GetByCompanyIdAsync(request.CompanyId, cancellationToken);

            var response = new List<OfferResponse>();

            foreach (var offer in offers)
            {
                var application = await _unitOfWork.JobApplications.GetByIdAsync(offer.JobApplicationId, cancellationToken);

                var job = application != null ? await _unitOfWork.Jobs.GetByIdAsync(application.JobId, cancellationToken) : null;

                var candidate = application != null ? await _unitOfWork.CandidateProfiles.GetByIdAsync(application.CandidateProfileId, cancellationToken) : null;

                var user = candidate != null ? await _unitOfWork.Users.GetByIdAsync(candidate.UserId, cancellationToken) : null;

                var company = await _unitOfWork.Companies.GetByIdAsync(request.CompanyId, cancellationToken);

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
