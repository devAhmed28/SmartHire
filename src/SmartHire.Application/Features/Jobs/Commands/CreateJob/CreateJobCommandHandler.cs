using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Jobs;
using SmartHire.Domain.Entities;

namespace SmartHire.Application.Features.Jobs.Commands.CreateJob
{
    public class CreateJobCommandHandler : IRequestHandler<CreateJobCommand, Result<JobResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateJobCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<JobResponse>> Handle(CreateJobCommand request, CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.Companies.GetByIdAsync(request.CompanyId, cancellationToken);

            if (company == null)
            {
                return Error.NotFound("Company");
            }

            var job = new Job(
                request.CompanyId,
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

            await _unitOfWork.Jobs.AddAsync(job, cancellationToken);

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

            var companyName = company.CompanyName;

            var response = new JobResponse
            {
                Id = job.Id,
                CompanyId = job.CompanyId,
                CompanyName = companyName,
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
