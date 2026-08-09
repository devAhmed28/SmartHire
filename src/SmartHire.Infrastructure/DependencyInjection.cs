using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Interfaces.Repositories;
using SmartHire.Infrastructure.Common.Settings;
using SmartHire.Infrastructure.Persistence.Context;
using SmartHire.Infrastructure.Persistence.Repositories;
using SmartHire.Infrastructure.Services;
using System.Text;

namespace SmartHire.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // JWT settings
            var jwtSettings = new JwtSettings();
            configuration.Bind(nameof(JwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);

            var cloudinarySettings = new CloudinarySettings();
            configuration.Bind(nameof(CloudinarySettings), cloudinarySettings);
            services.AddSingleton(cloudinarySettings);

            services.Configure<CloudinarySettings>(configuration.GetSection("Cloudinary"));

            // add authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            // add DB
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            });

            // repositories 
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
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<ICandidateSkillRepository, CandidateSkillRepository>();

            // add services
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IFileUploadService, CloudinaryService>();


            return services;
        }
    }
}
