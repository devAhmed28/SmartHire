using SmartHire.Domain.Enums;

namespace SmartHire.Application.DTOs.Companies
{
    public class UpdateCompanyRequest
    {
        public string CompanyName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;
        public string WebsiteUrl { get; set; } = string.Empty;
        public string? LinkedInUrl { get; set; }
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public int FoundedYear { get; set; }
        public CompanySize CompanySize { get; set; }
    }
}
