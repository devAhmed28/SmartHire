using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Admin;

namespace SmartHire.Application.Features.Admin.Queries.GetAllCompanies
{
    public class GetAllCompaniesQuery : IRequest<Result<List<CompanyAdminResponse>>>
    {
    }
}
