using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Candidate;

namespace SmartHire.Application.Features.Candidates.Queries.GetCandidateProfile
{
    public class GetCandidateProfileQuery : IRequest<Result<CandidateProfileResponse>>
    {
        public Guid UserId { get; set; }
    }
}
