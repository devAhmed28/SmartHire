using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Companies;

namespace SmartHire.Application.Features.Companies.Queries.GetCompanyById
{
    public class GetCompanyByIdQuery : IRequest<Result<CompanyProfileResponse>>
    {
        public Guid CompanyId { get; set; }
    }
}
