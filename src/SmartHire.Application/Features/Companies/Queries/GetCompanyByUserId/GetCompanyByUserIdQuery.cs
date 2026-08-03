using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Domain.Entities;

namespace SmartHire.Application.Features.Companies.Queries.GetCompanyByUserId
{
    public class GetCompanyByUserIdQuery : IRequest<Result<Company>>
    {
        public Guid UserId { get; set; }
    }
}
