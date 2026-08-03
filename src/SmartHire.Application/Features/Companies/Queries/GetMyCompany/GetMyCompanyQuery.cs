using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Companies;

namespace SmartHire.Application.Features.Companies.Queries.GetMyCompany
{
    public class GetMyCompanyQuery : IRequest<Result<CompanyProfileResponse>>
    {
        public Guid UserId { get; set; }
    }
}
