using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.SavedJobs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartHire.Application.Features.SavedJobs.Queries.GetSavedJobs
{
    public class GetSavedJobsQueryHandler : IRequestHandler<GetSavedJobsQuery, Result<List<SavedJobResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetSavedJobsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<SavedJobResponse>>> Handle(GetSavedJobsQuery request, CancellationToken cancellationToken)
        {
            var savedJobs = await _unitOfWork.SavedJobs.GetWithJobDetailsAsync(request.CandidateId, cancellationToken);

            var response = savedJobs.Select(sj => new SavedJobResponse
            {
                Id = sj.Id,
                JobId = sj.JobId,
                JobTitle = sj.Job.Title,
                CompanyName = sj.Job.Company.CompanyName,
                Location = sj.Job.Location,
                SalaryMin = sj.Job.SalaryMin,
                SalaryMax = sj.Job.SalaryMax,
                SavedAt = sj.CreatedAt
            }).ToList();

            return Result.Success(response);
        }
    }
}
