using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Candidate;

namespace SmartHire.Application.Features.Candidates.Commands.AddSkill
{
    public class AddSkillCommandHandler : IRequestHandler<AddSkillCommand, Result<CandidateProfileResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddSkillCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CandidateProfileResponse>> Handle(AddSkillCommand request, CancellationToken cancellationToken)
        {
            var candidateProfile = await _unitOfWork.CandidateProfiles.GetByUserIdAsync(request.UserId, cancellationToken);

            if (candidateProfile == null)
            {
                return Error.NotFound("Candidate profile not found for this user");
            }

            var skill = await _unitOfWork.Skills.GetOrCreateAsync(request.SkillName, cancellationToken);

            var existingSkill = candidateProfile.CandidateSkills
                .FirstOrDefault(cs => cs.SkillId == skill.Id);

            if (existingSkill != null)
            {
                return Error.Conflict($"Skill '{request.SkillName}' already added to your profile");
            }

            var candidateSkill = new CandidateSkill(
                candidateProfile.Id,
                skill.Id,
                request.YearsOfExperience
            );

            await _unitOfWork.CandidateSkills.AddAsync(candidateSkill, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var user = await _unitOfWork.Users.GetByIdAsync(candidateProfile.UserId, cancellationToken);

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
