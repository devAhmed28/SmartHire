using SmartHire.Domain.Enums;

namespace SmartHire.Application.DTOs.Jobs
{
    public class SearchJobsRequest
    {
        public string? SearchTerm { get; set; }
        public JobType? JobType { get; set; }
        public WorkMode? WorkMode { get; set; }
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public string? Location { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
