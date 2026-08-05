using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Interviews;

namespace SmartHire.Application.Features.Interviews.Queries.GetInterviewById
{
    public class GetInterviewByIdQuery : IRequest<Result<InterviewResponse>>
    {
        public Guid InterviewId { get; set; }
    }
}
