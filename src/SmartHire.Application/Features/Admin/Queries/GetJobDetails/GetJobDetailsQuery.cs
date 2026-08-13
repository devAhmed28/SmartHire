using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Admin;

namespace SmartHire.Application.Features.Admin.Queries.GetJobDetails
{
    public class GetJobDetailsQuery : IRequest<Result<JobAdminResponse>>
    {
        public Guid JobId { get; set; }
    }
}
