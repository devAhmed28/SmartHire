using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SmartHire.Application.Common.Behaviors;
using System.Reflection;

namespace SmartHire.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(
                    Assembly.GetExecutingAssembly());
            });

            services.AddValidatorsFromAssembly(
                Assembly.GetExecutingAssembly()
            );

            services.AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(ValidationBehavior<,>)
            );

            return services;
        }
    }
}
