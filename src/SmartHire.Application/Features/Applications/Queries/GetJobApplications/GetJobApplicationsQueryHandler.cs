using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Applications;

namespace SmartHire.Application.Features.Applications.Queries.GetJobApplications
{
    public class GetJobApplicationsQueryHandler : IRequestHandler<GetJobApplicationsQuery, Result<List<ApplicationResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetJobApplicationsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<ApplicationResponse>>> Handle(GetJobApplicationsQuery request, CancellationToken cancellationToken)
        {
            var job = await _unitOfWork.Jobs.GetByIdAsync(request.JobId, cancellationToken);

            if (job == null)
            {
                return Error.NotFound("Job");
            }

            if (job.CompanyId != request.CompanyId)
            {
                return Error.Forbidden("You do not have permission to view applications for this job");
            }

            var company = await _unitOfWork.Companies.GetByIdAsync(job.CompanyId, cancellationToken);

            var applications = await _unitOfWork.JobApplications.GetByJobIdAsync(
                request.JobId,
                cancellationToken
            );

            var response = new List<ApplicationResponse>();

            foreach (var application in applications)
            {
                var candidate = await _unitOfWork.CandidateProfiles.GetByIdAsync(application.CandidateProfileId, cancellationToken);

                var user = candidate != null ? await _unitOfWork.Users.GetByIdAsync(candidate.UserId, cancellationToken) : null;

                response.Add(new ApplicationResponse
                {
                    Id = application.Id,
                    JobId = application.JobId,
                    JobTitle = job.Title,
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
