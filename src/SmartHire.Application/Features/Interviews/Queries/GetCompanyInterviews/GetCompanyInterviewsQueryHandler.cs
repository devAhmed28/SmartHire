using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Interviews;

namespace SmartHire.Application.Features.Interviews.Queries.GetCompanyInterviews
{
    public class GetCompanyInterviewsQueryHandler : IRequestHandler<GetCompanyInterviewsQuery, Result<List<InterviewResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCompanyInterviewsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<InterviewResponse>>> Handle(GetCompanyInterviewsQuery request, CancellationToken cancellationToken)
        {
            var interviews = await _unitOfWork.Interviews.GetUpcomingForCompanyAsync(request.CompanyId, cancellationToken);

            var response = new List<InterviewResponse>();

            foreach (var interview in interviews)
            {
                var application = await _unitOfWork.JobApplications.GetByIdAsync(interview.JobApplicationId, cancellationToken);

                var job = application != null ? await _unitOfWork.Jobs.GetByIdAsync(application.JobId, cancellationToken) : null;

                var candidate = application != null ? await _unitOfWork.CandidateProfiles.GetByIdAsync(application.CandidateProfileId, cancellationToken) : null;

                var user = candidate != null ? await _unitOfWork.Users.GetByIdAsync(candidate.UserId, cancellationToken) : null;

                var company = await _unitOfWork.Companies.GetByIdAsync(request.CompanyId, cancellationToken);

                response.Add(new InterviewResponse
                {
                    Id = interview.Id,
                    ApplicationId = interview.JobApplicationId,
                    JobTitle = job?.Title ?? string.Empty,
                    CompanyName = company?.CompanyName ?? string.Empty,
                    CandidateName = user != null ? $"{user.FirstName} {user.LastName}" : string.Empty,
                    CandidateEmail = user?.Email ?? string.Empty,
                    ScheduledDate = interview.InterviewDate,
                    InterviewType = interview.Type,
                    Status = interview.Status,
                    MeetingLink = interview.MeetingLink,
                    Notes = interview.Notes,
                    CreatedAt = interview.CreatedAt,
                    UpdatedAt = interview.UpdatedAt
                });
            }

            return Result.Success(response);
        }
    }
}
