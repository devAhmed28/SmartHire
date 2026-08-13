using SmartHire.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using SmartHire.API.Middleware;
using SmartHire.Infrastructure;
using SmartHire.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SmartHire.Infrastructure.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

// ============ 1. Configure Serilog ============
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        path: "logs/smarthire-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();

// ============ 2. Add services ============

builder.Services.AddControllers();

// HttpContextAccessor (for CurrentUserService)
builder.Services.AddHttpContextAccessor();

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
    {
        Title = "SmartHire API",
        Version = "v1",
        Description = "SmartHire Recruitment Platform API"
    });
});

var app = builder.Build();

// ============ Seed admin user ============

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await SeedData.SeedAdminUserAsync(context);
}

// ============ 3. Configure pipeline ============

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(x => new
            {
                name = x.Key,
                status = x.Value.Status.ToString(),
                description = x.Value.Description,
                duration = x.Value.Duration.TotalMilliseconds
            }),
            totalDuration = report.TotalDuration.TotalMilliseconds
        };

        await context.Response.WriteAsJsonAsync(response);
    }
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();


try
{
    Log.Information("Starting SmartHire API...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start");
}
finally
{
    Log.CloseAndFlush();
}