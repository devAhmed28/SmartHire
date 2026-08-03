namespace SmartHire.Application.DTOs.Companies
{
    public class CompanyProfileResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;
        public string WebsiteUrl { get; set; } = string.Empty;
        public string? LogoUrl { get; set; }
        public string? LinkedInUrl { get; set; }
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public int FoundedYear { get; set; }
        public bool IsVerified { get; set; }
        public string CompanySize { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty; // from user
        public string PhoneNumber { get; set; } = string.Empty; // from user
    }
}
