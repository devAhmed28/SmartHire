using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Interviews;
using SmartHire.Domain.Entities;
using SmartHire.Domain.Enums;

namespace SmartHire.Application.Features.Interviews.Commands.ScheduleInterview
{
    public class ScheduleInterviewCommandHandler : IRequestHandler<ScheduleInterviewCommand, Result<InterviewResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ScheduleInterviewCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<InterviewResponse>> Handle(ScheduleInterviewCommand request, CancellationToken cancellationToken)
        {
            var application = await _unitOfWork.JobApplications.GetByIdAsync(request.ApplicationId, cancellationToken);

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
                return Error.Forbidden("You do not have permission to schedule interviews for this application");
            }

            if (application.Status != ApplicationStatus.Reviewing && application.Status != ApplicationStatus.Pending)
            {
                return Error.Validation($"Cannot schedule interview for application with status: {application.Status}");
            }

            var interview = new Interview(
                request.ApplicationId,
                request.ScheduledDate,
                request.InterviewType,
                request.MeetingLink,
                request.Notes
            );

            await _unitOfWork.Interviews.AddAsync(interview, cancellationToken);

            application.UpdateStatus(ApplicationStatus.Interview);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var candidate = await _unitOfWork.CandidateProfiles.GetByIdAsync(application.CandidateProfileId, cancellationToken);

            var user = candidate != null ? await _unitOfWork.Users.GetByIdAsync(candidate.UserId, cancellationToken) : null;

            var company = await _unitOfWork.Companies.GetByIdAsync(job.CompanyId, cancellationToken);

            if (user != null)
            {
                var notification = new Notification(
                    user.Id,
                    "Interview Scheduled",
                    $"Your interview for '{job.Title}' has been scheduled for {interview.InterviewDate.ToString("MMM dd, yyyy hh:mm tt")}",
                    NotificationType.InterviewInvitation
                );

                await _unitOfWork.Notifications.AddAsync(notification, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            var response = new InterviewResponse
            {
                Id = interview.Id,
                ApplicationId = interview.JobApplicationId,
                JobTitle = job.Title,
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
