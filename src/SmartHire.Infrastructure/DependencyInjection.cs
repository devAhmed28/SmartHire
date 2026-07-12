using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Interfaces.Repositories;
using SmartHire.Infrastructure.Persistence.Context;
using SmartHire.Infrastructure.Persistence.Repositories;

namespace SmartHire.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IJobRepository, JobRepository>();
            services.AddScoped<ICandidateProfileRepository, CandidateProfileRepository>();
            services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();
            services.AddScoped<ISkillRepository, SkillRepository>();
            services.AddScoped<IInterviewRepository, InterviewRepository>();
            services.AddScoped<IOfferRepository, OfferRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<ISavedJobRepository, SavedJobRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            return services;
        }
    }
}
