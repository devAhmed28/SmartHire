using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Companies;

namespace SmartHire.Application.Features.Companies.Queries.GetCompany
{
    public class GetCompanyByIdQuery : IRequest<Result<CompanyProfileResponse>>
    {
        public Guid CompanyId { get; set; }
    }
    
    public class GetMyCompanyQuery : IRequest<Result<CompanyProfileResponse>>
    {
        public Guid UserId { get; set; }
    }
}
