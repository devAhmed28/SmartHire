using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Jobs;
using SmartHire.Domain.Enums;

namespace SmartHire.Application.Features.Jobs.Commands.UpdateJob
{
    public class UpdateJobCommand : IRequest<Result<JobResponse>>
    {
        public Guid JobId { get; set; }
        public Guid CompanyId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Responsibilities { get; set; } = string.Empty;
        public string Requirements { get; set; } = string.Empty;
        public decimal SalaryMin { get; set; }
        public decimal SalaryMax { get; set; }
        public string Location { get; set; } = string.Empty;
        public int Vacancies { get; set; }
        public Currency Currency { get; set; }
        public JobType JobType { get; set; }
        public WorkMode WorkMode { get; set; }
        public DateTime ExpirationDate { get; set; }
        public List<Guid> SkillIds { get; set; } = new List<Guid>();
    }
}
