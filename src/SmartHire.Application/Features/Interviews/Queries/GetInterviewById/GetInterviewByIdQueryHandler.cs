using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Interviews;

namespace SmartHire.Application.Features.Interviews.Queries.GetInterviewById
{
    public class GetInterviewByIdQueryHandler : IRequestHandler<GetInterviewByIdQuery, Result<InterviewResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetInterviewByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<InterviewResponse>> Handle(GetInterviewByIdQuery request, CancellationToken cancellationToken)
        {
            var interview = await _unitOfWork.Interviews.GetByIdAsync(request.InterviewId, cancellationToken);
            if (interview == null)
            {
                return Error.NotFound("Interview");
            }

            var application = await _unitOfWork.JobApplications.GetByIdAsync(interview.JobApplicationId, cancellationToken);

            var job = application != null ? await _unitOfWork.Jobs.GetByIdAsync(application.JobId, cancellationToken) : null;

            var candidate = application != null ? await _unitOfWork.CandidateProfiles.GetByIdAsync(application.CandidateProfileId, cancellationToken) : null;

            var user = candidate != null ? await _unitOfWork.Users.GetByIdAsync(candidate.UserId, cancellationToken) : null;

            var company = job != null ? await _unitOfWork.Companies.GetByIdAsync(job.CompanyId, cancellationToken) : null;

            var response = new InterviewResponse
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
            };

            return Result.Success(response);
        }
    }
}
