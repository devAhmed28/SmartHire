using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Jobs;
using SmartHire.Domain.Entities;

namespace SmartHire.Application.Features.Jobs.Commands.UpdateJob
{
    public class UpdateJobCommandHandler : IRequestHandler<UpdateJobCommand, Result<JobResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateJobCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<JobResponse>> Handle(UpdateJobCommand request,  CancellationToken cancellationToken)
        {
            var job = await _unitOfWork.Jobs.GetByIdAsync(request.JobId, cancellationToken);

            if (job == null)
            {
                return Error.NotFound("Job");
            }

            if (job.CompanyId != request.CompanyId)
            {
                return Error.Forbidden("You do not have permission to update this job");
            }

            job.UpdateJob(
            request.Title,
            request.Description,
            request.Responsibilities,
            request.Requirements,
            request.SalaryMin,
            request.SalaryMax,
            request.Location,
            request.Vacancies,
            request.Currency,
            request.JobType,
            request.WorkMode,
            request.ExpirationDate
            );

            // remove existing skills then update with the newest
            var existingSkills = job.JobSkills.ToList();
            
            foreach (var skill in existingSkills)
            {
                job.JobSkills.Remove(skill);
            }

            if (request.SkillIds.Any())
            {
                foreach (var skillId in request.SkillIds)
                {
                    var skill = await _unitOfWork.Skills.GetByIdAsync(skillId, cancellationToken);
                    if (skill != null)
                    {
                        var jobSkill = new JobSkill(job.Id, skillId, true);
                        job.JobSkills.Add(jobSkill);
                    }
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var company = await _unitOfWork.Companies.GetByIdAsync(job.CompanyId, cancellationToken);

            var response = new JobResponse
            {
                Id = job.Id,
                CompanyId = job.CompanyId,
                CompanyName = company?.CompanyName ?? string.Empty,
                Title = job.Title,
                Description = job.Description,
                Responsibilities = job.Responsibilities,
                Requirements = job.Requirements,
                SalaryMin = job.SalaryMin,
                SalaryMax = job.SalaryMax,
                Location = job.Location,
                Vacancies = job.Vacancies,
                Currency = job.Currency,
                JobType = job.JobType,
                WorkMode = job.WorkMode,
                JobStatus = job.JobStatus,
                ExpirationDate = job.ExpirationDate,
                CreatedAt = job.CreatedAt,
                Skills = new List<string>()
            };

            return Result.Success(response);
        }
    }
}
