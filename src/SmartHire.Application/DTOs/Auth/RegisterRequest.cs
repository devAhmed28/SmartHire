using SmartHire.Domain.Enums;

namespace SmartHire.Application.DTOs.Auth
{
    public class RegisterRequest
    {
        public AccountType AccountType { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public CompanyInfo? Company { get; set; }
        public CandidateInfo? Candidate { get; set; }
    }

    public class CandidateInfo
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    public class CompanyInfo
    {
        public string CompanyName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;
        public CompanySize CompanySize { get; set; }
    }
}
