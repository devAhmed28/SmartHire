using SmartHire.Domain.Common;

namespace SmartHire.Domain.Entities
{
    public class CandidateProfile : BaseAuditableEntity
    {
        public Guid UserId { get; private set; }
        public string Bio {  get; private set; } = string.Empty;
        public string CVUrl {  get; private set; } = string.Empty;
        public string GitHubUrl {  get; private set; } = string.Empty;
        public string LinkedInUrl {  get; private set; } = string.Empty;
        public string? PortfolioUrl {  get; private set; }
        public int YearsOfExperience {  get; private set; }
        public string CurrentPosition { get; private set; } = string.Empty;
        public string CurrentLocation { get; private set; } = string.Empty;
        public decimal ExpectedSalary { get; private set; }
        public bool IsOpenToWork { get; private set; } = true;
        public User User { get; private set; } = null!;
    }
}
