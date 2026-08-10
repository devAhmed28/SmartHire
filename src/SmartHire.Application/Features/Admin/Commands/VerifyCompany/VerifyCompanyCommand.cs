using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Admin;

namespace SmartHire.Application.Features.Admin.Commands.VerifyCompany
{
    public class VerifyCompanyCommand : IRequest<Result<CompanyAdminResponse>>
    {
        public Guid CompanyId { get; set; }
    }
}
