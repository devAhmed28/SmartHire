using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Candidate;

namespace SmartHire.Application.Features.Candidates.Queries.GetCandidateProfile
{
    public class GetCandidateProfileQueryHandler : IRequestHandler<GetCandidateProfileQuery, Result<CandidateProfileResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCandidateProfileQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CandidateProfileResponse>> Handle(GetCandidateProfileQuery request, CancellationToken cancellationToken)
        {
            var candidateProfile = await _unitOfWork.CandidateProfiles.GetWithSkillsByUserIdAsync(request.UserId, cancellationToken);

            if (candidateProfile == null)
            {
                return Error.NotFound("Candidate profile not found for thins user");
            }

            var user = await _unitOfWork.Users.GetByIdAsync(candidateProfile.UserId, cancellationToken);

            if (user == null)
            {
                return Error.NotFound("User");
            }

            var skills = candidateProfile.CandidateSkills
                .Select(cs => cs.Skill.Name)
                .ToList();

            var response = new CandidateProfileResponse
            {
                Id = candidateProfile.Id,
                UserId = candidateProfile.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Bio = candidateProfile.Bio,
                CVUrl = candidateProfile.CVUrl,
                GitHubUrl = candidateProfile.GitHubUrl,
                LinkedInUrl = candidateProfile.LinkedInUrl,
                PortfolioUrl = candidateProfile.PortfolioUrl,
                YearsOfExperience = candidateProfile.YearsOfExperience,
                CurrentPosition = candidateProfile.CurrentPosition,
                CurrentLocation = candidateProfile.CurrentLocation,
                ExpectedSalary = candidateProfile.ExpectedSalary,
                IsOpenToWork = candidateProfile.IsOpenToWork,
                Skills = skills
            };

            return Result.Success(response);
        }
    }
}
