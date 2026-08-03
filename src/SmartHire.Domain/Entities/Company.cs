using SmartHire.Domain.Common;
using SmartHire.Domain.Enums;

namespace SmartHire.Domain.Entities
{
    public class Company : BaseAuditableEntity
    {
        public Company(
        Guid userId,
        string companyName,
        string description,
        string industry,
        CompanySize companySize)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            CompanyName = companyName;
            Description = description;
            Industry = industry;
            CompanySize = companySize;
            IsVerified = false;
            WebsiteUrl = string.Empty;
            Address = string.Empty;
            City = string.Empty;
            Country = string.Empty;
            FoundedYear = DateTime.UtcNow.Year;
            CreatedAt = DateTime.UtcNow;
        }

        private Company() { }

        public Guid UserId { get; private set; }
        public string CompanyName { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string Industry { get; private set; } = string.Empty;
        public string WebsiteUrl { get; private set; } = string.Empty;
        public string? LogoUrl { get; private set; }
        public string? LinkedInUrl { get; private set; }
        public string Address { get; private set; } = string.Empty;
        public string City { get; private set; } = string.Empty;
        public string Country { get; private set; } = string.Empty;
        public int FoundedYear { get; private set; }
        public bool IsVerified { get; private set; }
        public CompanySize CompanySize { get; private set; }
        public User User { get; private set; } = null!;
        public ICollection<Job> Jobs { get; private set; } = new List<Job>();
        public ICollection<Review> Reviews { get; private set; } = new List<Review>();


        public void UpdateCompanyDetails(
        string companyName,
        string description,
        string industry,
        CompanySize companySize)
        {
            CompanyName = companyName;
            Description = description;
            Industry = industry;
            CompanySize = companySize;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateAddress(string address, string city, string country)
        {
            Address = address;
            City = city;
            Country = country;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateWebsite(string websiteUrl)
        {
            WebsiteUrl = websiteUrl;
            UpdatedAt = DateTime.UtcNow;
        }
        
        public void UpdateLinkedIn(string? linkedInUrl)
        {
            LinkedInUrl = linkedInUrl;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateLogo(string logoUrl)
        {
            LogoUrl = logoUrl;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Verify()
        {
            IsVerified = true;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
