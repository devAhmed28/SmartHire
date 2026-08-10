using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Admin;

namespace SmartHire.Application.Features.Admin.Queries.GetDashboardStats
{
    public class GetDashboardStatsQueryHandler : IRequestHandler<GetDashboardStatsQuery, Result<AdminDashboardResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDashboardStatsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<AdminDashboardResponse>> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
        {
            var totalUsers = await _unitOfWork.Users.CountAsync(cancellationToken: cancellationToken);
            var totalCompanies = await _unitOfWork.Companies.CountAsync(cancellationToken: cancellationToken);
            var totalJobs = await _unitOfWork.Jobs.CountAsync(cancellationToken: cancellationToken);
            var totalApplications = await _unitOfWork.JobApplications.CountAsync(cancellationToken: cancellationToken);
            var totalInterviews = await _unitOfWork.Interviews.CountAsync(cancellationToken: cancellationToken);
            var totalOffers = await _unitOfWork.Offers.CountAsync(cancellationToken: cancellationToken);
            var totalReviews = await _unitOfWork.Reviews.CountAsync(cancellationToken: cancellationToken);

            var response = new AdminDashboardResponse
            {
                TotalUsers = totalUsers,
                TotalCompanies = totalCompanies,
                TotalJobs = totalJobs,
                TotalApplications = totalApplications,
                TotalInterviews = totalInterviews,
                TotalOffers = totalOffers,
                TotalReviews = totalReviews
            };

            return Result.Success(response);
        }
    }
}
