using SmartHire.Application.Common.Interfaces.Repositories;

namespace SmartHire.Application.Common.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        ICompanyRepository Companies { get; }
        IJobRepository Jobs { get; }
        ICandidateProfileRepository CandidateProfiles { get; }
        IJobApplicationRepository JobApplications { get; }
        ISkillRepository Skills { get; }
        IInterviewRepository Interviews { get; }
        IOfferRepository Offers { get; }
        IReviewRepository Reviews { get; }
        ISavedJobRepository SavedJobs { get; }
        INotificationRepository Notifications { get; }

        Task<int> SaveChanges(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}
