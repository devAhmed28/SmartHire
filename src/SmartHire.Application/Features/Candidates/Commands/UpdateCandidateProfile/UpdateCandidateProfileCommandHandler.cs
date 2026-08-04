using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Candidate;

namespace SmartHire.Application.Features.Candidates.Commands.UpdateCandidateProfile
{
    public class UpdateCandidateProfileCommandHandler : IRequestHandler<UpdateCandidateProfileCommand, Result<CandidateProfileResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCandidateProfileCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CandidateProfileResponse>> Handle(UpdateCandidateProfileCommand request, CancellationToken cancellationToken)
        {
            var candidateProfile = await _unitOfWork.CandidateProfiles.GetByUserIdAsync(request.UserId, cancellationToken);

            if (candidateProfile == null)
            {
                return Error.NotFound("Candidate profile not found for this user");
            }

            var user = await _unitOfWork.Users.GetByIdAsync(candidateProfile.UserId, cancellationToken);

            if (user == null)
            {
                return Error.NotFound("User");
            }

            candidateProfile.UpdateProfile(
                request.Bio,
                request.CurrentPosition,
                request.CurrentLocation,
                request.YearsOfExperience,
                request.ExpectedSalary
            );

            candidateProfile.UpdateSocialLinks(
                request.GitHubUrl,
                request.LinkedInUrl,
                request.PortfolioUrl
            );

            candidateProfile.SetOpenToWork(request.IsOpenToWork);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

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
