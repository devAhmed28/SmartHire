using SmartHire.Domain.Entities;
using SmartHire.Domain.Enums;

namespace SmartHire.Application.Common.Interfaces.Repositories
{
    public interface IJobApplicationRepository : IRepository<JobApplication>
    {
        Task<IReadOnlyList<JobApplication>> GetByJobIdAsync(Guid jobId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<JobApplication>> GetByCandidateProfileIdAsync(Guid candidateProfileId, CancellationToken cancellationToken = default);
        Task<JobApplication?> GetWithInterviewAndOfferAsync(Guid jobApplicationId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<JobApplication>> GetByStatusAsync(ApplicationStatus status, CancellationToken cancellationToken = default);
        Task<bool> HasAppliedAsync(Guid jobId, Guid candidateProfileId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<JobApplication>> GetCompanyApplicationsAsync(Guid companyId, CancellationToken cancellationToken = default);

    }
}
