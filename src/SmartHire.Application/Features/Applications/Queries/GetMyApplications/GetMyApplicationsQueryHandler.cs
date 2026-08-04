using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Applications;

namespace SmartHire.Application.Features.Applications.Queries.GetMyApplications
{
    public class GetMyApplicationsQueryHandler : IRequestHandler<GetMyApplicationsQuery, Result<List<ApplicationResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMyApplicationsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<ApplicationResponse>>> Handle(GetMyApplicationsQuery request, CancellationToken cancellationToken)
        {
            var applications = await _unitOfWork.JobApplications.GetByCandidateProfileIdAsync(
                request.CandidateId,
                cancellationToken
            );

            var response = new List<ApplicationResponse>();

            foreach (var application in applications)
            {
                var job = await _unitOfWork.Jobs.GetByIdAsync(application.JobId, cancellationToken);

                var company = job != null ? await _unitOfWork.Companies.GetByIdAsync(job.CompanyId, cancellationToken) : null;

                var candidate = await _unitOfWork.CandidateProfiles.GetByIdAsync(application.CandidateProfileId, cancellationToken);

                var user = candidate != null ? await _unitOfWork.Users.GetByIdAsync(candidate.UserId, cancellationToken) : null;

                response.Add(new ApplicationResponse
                {
                    Id = application.Id,
                    JobId = application.JobId,
                    JobTitle = job?.Title ?? string.Empty,
                    CompanyName = company?.CompanyName ?? string.Empty,
                    CandidateName = user != null ? $"{user.FirstName} {user.LastName}" : string.Empty,
                    CandidateEmail = user?.Email ?? string.Empty,
                    Status = application.Status,
                    CoverLetter = application.CoverLetter,
                    AppliedAt = application.AppliedAt,
                    UpdatedAt = application.UpdatedAt
                });
            }

            return Result.Success(response);
        }
    }
}
