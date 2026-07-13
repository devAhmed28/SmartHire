using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using SmartHire.API.Middleware;
using SmartHire.Infrastructure;
using SmartHire.Infrastructure.Persistence.Context;

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

// ============ 2. Add Health Checks ============
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>();

// ============ 2a. Add CORS ============
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()      // Allow any domain
                  .AllowAnyMethod()      // Allow GET, POST, PUT, DELETE, etc.
                  .AllowAnyHeader();     // Allow any headers
        });
});

// ============ 2b. Add Swagger ============
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ============ 2c. Add Infrastructure ============
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// ============ 3. Configure pipeline ============

// 1. Exception handling
app.UseMiddleware<ExceptionHandlingMiddleware>();

// 2. Request logging
app.UseMiddleware<RequestLoggingMiddleware>();

// 3. CORS - !! before UseAuthorization
app.UseCors("AllowAll");

// 4. Health Checks - !! before swagger
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
                duration = x.Value.Duration.TotalMilliseconds,
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

// ============ 4. Run ============

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