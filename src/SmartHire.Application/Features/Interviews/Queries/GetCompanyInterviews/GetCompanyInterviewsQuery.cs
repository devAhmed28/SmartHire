using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Interviews;

namespace SmartHire.Application.Features.Interviews.Queries.GetCompanyInterviews
{
    public class GetCompanyInterviewsQuery : IRequest<Result<List<InterviewResponse>>>
    {
        public Guid CompanyId { get; set; }
    }
}
