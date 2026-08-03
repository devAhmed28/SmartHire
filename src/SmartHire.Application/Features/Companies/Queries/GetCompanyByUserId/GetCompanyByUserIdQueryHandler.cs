using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Domain.Entities;

namespace SmartHire.Application.Features.Companies.Queries.GetCompanyByUserId
{
    public class GetCompanyByUserIdQueryHandler : IRequestHandler<GetCompanyByUserIdQuery, Result<Company>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCompanyByUserIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Company>> Handle(GetCompanyByUserIdQuery request, CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.Companies.GetByUserIdAsync(request.UserId, cancellationToken);

            if (company == null)
            {
                return Error.NotFound("Company not found for this user");
            }

            return Result.Success(company);
        }
    }
}
