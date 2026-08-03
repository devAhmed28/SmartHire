using SmartHire.Domain.Common;
using SmartHire.Domain.Enums;

namespace SmartHire.Domain.Entities
{
    public class Job : BaseAuditableEntity
    {
        public Job(
            Guid companyId,
            string title,
            string description,
            string responsibilities,
            string requirements,
            decimal salaryMin,
            decimal salaryMax,
            string location,
            int vacancies,
            Currency currency,
            JobType jobType,
            WorkMode workMode,
            DateTime expirationDate
        )
        {
            Id = Guid.NewGuid();
            CompanyId = companyId;
            Title = title;
            Description = description;
            Responsibilities = responsibilities;
            Requirements = requirements;
            SalaryMin = salaryMin;
            SalaryMax = salaryMax;
            Location = location;
            Vacancies = vacancies;
            Currency = currency;
            JobType = jobType;
            WorkMode = workMode;
            JobStatus = JobStatus.Draft; // Default status when created
            ExpirationDate = expirationDate;
            CreatedAt = DateTime.UtcNow;
        }

        private Job() { }

        public Guid CompanyId { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string Responsibilities { get; private set; } = string.Empty;
        public string Requirements { get; private set; } = string.Empty;
        public decimal SalaryMin { get; private set; }
        public decimal SalaryMax { get; private set; }
        public string Location { get; private set; } = string.Empty;
        public int Vacancies { get; private set; }
        public Currency Currency { get; private set; }
        public JobType JobType { get; private set; }
        public WorkMode WorkMode { get; private set; }
        public JobStatus JobStatus { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public Company Company { get; private set; } = null!;
        public ICollection<JobSkill> JobSkills { get; private set; } = new List<JobSkill>();
        public ICollection<JobApplication> JobApplications { get; private set; } = new List<JobApplication>();
        public ICollection<SavedJob> SavedJobs { get; private set; } = new List<SavedJob>();

        public void UpdateJob(
        string title,
        string description,
        string responsibilities,
        string requirements,
        decimal salaryMin,
        decimal salaryMax,
        string location,
        int vacancies,
        Currency currency,
        JobType jobType,
        WorkMode workMode,
        DateTime expirationDate)
        {
            Title = title;
            Description = description;
            Responsibilities = responsibilities;
            Requirements = requirements;
            SalaryMin = salaryMin;
            SalaryMax = salaryMax;
            Location = location;
            Vacancies = vacancies;
            Currency = currency;
            JobType = jobType;
            WorkMode = workMode;
            ExpirationDate = expirationDate;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
