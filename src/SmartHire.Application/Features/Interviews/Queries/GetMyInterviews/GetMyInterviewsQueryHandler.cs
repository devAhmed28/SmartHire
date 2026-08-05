using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Interviews;

namespace SmartHire.Application.Features.Interviews.Queries.GetMyInterviews
{
    public class GetMyInterviewsQueryHandler : IRequestHandler<GetMyInterviewsQuery, Result<List<InterviewResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMyInterviewsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<InterviewResponse>>> Handle(GetMyInterviewsQuery request, CancellationToken cancellationToken)
        {
            var candidate = await _unitOfWork.CandidateProfiles.GetByIdAsync(request.CandidateId, cancellationToken);

            if (candidate == null)
            {
                return Error.NotFound("Candidate profile");
            }

            var interviews = await _unitOfWork.Interviews.GetUpcomingForCandidateAsync(candidate.Id, cancellationToken);

            var response = new List<InterviewResponse>();

            foreach (var interview in interviews)
            {
                var application = await _unitOfWork.JobApplications.GetByIdAsync(interview.JobApplicationId, cancellationToken);

                var job = application != null ? await _unitOfWork.Jobs.GetByIdAsync(application.JobId, cancellationToken) : null;

                var company = job != null ? await _unitOfWork.Companies.GetByIdAsync(job.CompanyId, cancellationToken) : null;

                response.Add(new InterviewResponse
                {
                    Id = interview.Id,
                    ApplicationId = interview.JobApplicationId,
                    JobTitle = job?.Title ?? string.Empty,
                    CompanyName = company?.CompanyName ?? string.Empty,
                    CandidateName = $"{candidate.User.FirstName} {candidate.User.LastName}",
                    CandidateEmail = candidate.User.Email,
                    ScheduledDate = interview.InterviewDate,
                    InterviewType = interview.Type,
                    Status = interview.Status,
                    MeetingLink = interview.MeetingLink,
                    Notes = interview.Notes,
                    CreatedAt = interview.CreatedAt,
                    UpdatedAt = interview.UpdatedAt,
                });
            }

            return Result.Success(response);
        }
    }
}
