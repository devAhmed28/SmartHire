using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Companies;
using SmartHire.Domain.Enums;

namespace SmartHire.Application.Features.Companies.Commands.UpdateCompany
{
    public class UpdateCompanyCommand : IRequest<Result<CompanyProfileResponse>>
    {
        public Guid UserId { get; set; }
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
