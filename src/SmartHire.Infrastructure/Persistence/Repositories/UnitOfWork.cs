using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Interfaces.Repositories;
using SmartHire.Infrastructure.Persistence.Context;

namespace SmartHire.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;
    private bool _disposed;

    private IUserRepository? _userRepository;
    private ICompanyRepository? _companyRepository;
    private IJobRepository? _jobRepository;
    private ICandidateProfileRepository? _candidateProfileRepository;
    private IJobApplicationRepository? _jobApplicationRepository;
    private ISkillRepository? _skillRepository;
    private IInterviewRepository? _interviewRepository;
    private IOfferRepository? _offerRepository;
    private IReviewRepository? _reviewRepository;
    private ISavedJobRepository? _savedJobRepository;
    private INotificationRepository? _notificationRepository;
    private IRefreshTokenRepository? _refreshTokenRepository;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IUserRepository Users =>
        _userRepository ??= new UserRepository(_context);

    public ICompanyRepository Companies =>
        _companyRepository ??= new CompanyRepository(_context);

    public IJobRepository Jobs =>
        _jobRepository ??= new JobRepository(_context);

    public ICandidateProfileRepository CandidateProfiles =>
        _candidateProfileRepository ??= new CandidateProfileRepository(_context);

    public IJobApplicationRepository JobApplications =>
        _jobApplicationRepository ??= new JobApplicationRepository(_context);

    public ISkillRepository Skills =>
        _skillRepository ??= new SkillRepository(_context);

    public IInterviewRepository Interviews =>
        _interviewRepository ??= new InterviewRepository(_context);

    public IOfferRepository Offers =>
        _offerRepository ??= new OfferRepository(_context);

    public IReviewRepository Reviews =>
        _reviewRepository ??= new ReviewRepository(_context);

    public ISavedJobRepository SavedJobs =>
        _savedJobRepository ??= new SavedJobRepository(_context);

    public INotificationRepository Notifications =>
        _notificationRepository ??= new NotificationRepository(_context);

    public IRefreshTokenRepository RefreshTokens =>
        _refreshTokenRepository ??= new RefreshTokenRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    /// <summary>
    /// Disposes the UnitOfWork and releases resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _transaction?.Dispose();
                _context.Dispose();
            }
            _disposed = true;
        }
    }
}