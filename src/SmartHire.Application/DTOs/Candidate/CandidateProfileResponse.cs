namespace SmartHire.Application.DTOs.Candidate
{
    public class CandidateProfileResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string CVUrl { get; set; } = string.Empty;
        public string GitHubUrl { get; set; } = string.Empty;
        public string LinkedInUrl { get; set; } = string.Empty;
        public string? PortfolioUrl { get; set; }
        public int YearsOfExperience { get; set; }
        public string CurrentPosition { get; set; } = string.Empty;
        public string CurrentLocation { get; set; } = string.Empty;
        public decimal ExpectedSalary { get; set; }
        public bool IsOpenToWork { get; set; }
        public List<string> Skills { get; set; } = new List<string>();
    }
}
