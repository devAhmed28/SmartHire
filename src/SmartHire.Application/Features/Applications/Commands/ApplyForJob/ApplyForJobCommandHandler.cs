using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Applications;
using SmartHire.Domain.Entities;
using SmartHire.Domain.Enums;

namespace SmartHire.Application.Features.Applications.Commands.ApplyForJob
{
    public class ApplyForJobCommandHandler : IRequestHandler<ApplyForJobCommand, Result<ApplicationResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApplyForJobCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ApplicationResponse>> Handle(ApplyForJobCommand request, CancellationToken cancellationToken)
        {
            var candidate = await _unitOfWork.CandidateProfiles.GetByUserIdAsync(request.CandidateId, cancellationToken);

            if (candidate == null)
            {
                return Error.NotFound("Candidate profile");
            }

            var job = await _unitOfWork.Jobs.GetByIdAsync(request.JobId, cancellationToken);

            if (job == null)
            {
                return Error.NotFound("Job");
            }

            if (job.JobStatus != JobStatus.Published)
            {
                return Error.Validation("This job is not available for applications");
            }

            if (job.ExpirationDate < DateTime.UtcNow)
            {
                return Error.Validation("This job has expired");
            }

            var existingApplication = await _unitOfWork.JobApplications.HasAppliedAsync(
                request.JobId,
                candidate.Id,
                cancellationToken
            );

            if (existingApplication)
            {
                return Error.Conflict("You have already applied for this job");
            }

            var application = new JobApplication(
                candidate.Id,
                request.JobId,
                request.CoverLetter
            );

            await _unitOfWork.JobApplications.AddAsync(application, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var user = await _unitOfWork.Users.GetByIdAsync(candidate.UserId, cancellationToken);

            var company = await _unitOfWork.Companies.GetByIdAsync(job.CompanyId, cancellationToken);

            var response = new ApplicationResponse
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
            };

            return Result.Success(response);
        }
    }
}
