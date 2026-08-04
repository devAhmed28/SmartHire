using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;

namespace SmartHire.Application.Features.Candidates.Commands.RemoveSkill
{
    public class RemoveSkillCommandHandler : IRequestHandler<RemoveSkillCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveSkillCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(RemoveSkillCommand request, CancellationToken cancellationToken)
        {
            var candidateProfile = await _unitOfWork.CandidateProfiles.GetWithSkillsByUserIdAsync(request.UserId, cancellationToken);

            if (candidateProfile == null)
            {
                return Error.NotFound("Candidate profile not found for this user");
            }

            var skill = await _unitOfWork.Skills.GetByNameAsync(request.SkillName, cancellationToken);

            if (skill == null)
            {
                return Error.NotFound($"Skill '{request.SkillName}' not found");
            }

            var candidateSkill = candidateProfile.CandidateSkills
                .FirstOrDefault(cs => cs.SkillId == skill.Id);

            if (candidateSkill == null)
            {
                return Error.NotFound($"Skill '{request.SkillName}' is not found in your profile");
            }

            await _unitOfWork.CandidateSkills.DeleteAsync(candidateSkill);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
