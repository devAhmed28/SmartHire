using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Applications;

namespace SmartHire.Application.Features.Applications.Queries.GetMyApplications
{
    public class GetMyApplicationsQuery : IRequest<Result<List<ApplicationResponse>>>
    {
        public Guid CandidateId { get; set; }
    }
}
