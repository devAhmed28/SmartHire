using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Interviews;

namespace SmartHire.Application.Features.Interviews.Commands.UpdateInterviewStatus
{
    public class UpdateInterviewStatusCommandHandler : IRequestHandler<UpdateInterviewStatusCommand, Result<InterviewResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateInterviewStatusCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<InterviewResponse>> Handle(UpdateInterviewStatusCommand request, CancellationToken cancellationToken)
        {
            var interview = await _unitOfWork.Interviews.GetByIdAsync(request.InterviewId, cancellationToken);

            if (interview == null)
            {
                return Error.NotFound("Interview");
            }

            var application = await _unitOfWork.JobApplications.GetByIdAsync(interview.JobApplicationId, cancellationToken);

            if (application == null)
            {
                return Error.NotFound("Application");
            }

            var job = await _unitOfWork.Jobs.GetByIdAsync(application.JobId, cancellationToken);

            if (job == null || job.CompanyId != request.CompanyId)
            {
                return Error.Forbidden("You do not have permission to update this interview");
            }

            interview.UpdateStatus(request.Status, request.Notes);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken); 

            var candidate = await _unitOfWork.CandidateProfiles.GetByIdAsync(application.CandidateProfileId, cancellationToken);

            var user = candidate != null ? await _unitOfWork.Users.GetByIdAsync(candidate.UserId, cancellationToken) : null;

            var company = await _unitOfWork.Companies.GetByIdAsync(job.CompanyId, cancellationToken);

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
