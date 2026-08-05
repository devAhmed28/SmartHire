using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Interviews;

namespace SmartHire.Application.Features.Interviews.Queries.GetMyInterviews
{
    public class GetMyInterviewsQuery : IRequest<Result<List<InterviewResponse>>>
    {
        public Guid CandidateId { get; set; }
    }
}
