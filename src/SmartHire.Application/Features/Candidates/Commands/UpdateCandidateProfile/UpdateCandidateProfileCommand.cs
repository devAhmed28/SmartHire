using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Candidate;

namespace SmartHire.Application.Features.Candidates.Commands.UpdateCandidateProfile
{
    public class UpdateCandidateProfileCommand : IRequest<Result<CandidateProfileResponse>>
    {
        public Guid UserId { get; set; }
        public string Bio { get; set; } = string.Empty;
        public string GitHubUrl { get; set; } = string.Empty;
        public string LinkedInUrl { get; set; } = string.Empty;
        public string? PortfolioUrl { get; set; }
        public int YearsOfExperience { get; set; }
        public string CurrentPosition { get; set; } = string.Empty;
        public string CurrentLocation { get; set; } = string.Empty;
        public decimal ExpectedSalary { get; set; }
        public bool IsOpenToWork { get; set; }
    }
}
