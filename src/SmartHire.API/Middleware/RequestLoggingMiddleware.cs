using System.Diagnostics;
using System.Timers;

namespace SmartHire.API.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            // request info
            var method = context.Request.Method;
            var path = context.Request.Path;
            var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            try
            {
                // process request
                await _next(context);

                // log response
                stopwatch.Stop();
                var statusCode = context.Response.StatusCode;
                var elapseMs = stopwatch.ElapsedMilliseconds;

                _logger.LogInformation($"Request: {method} {path} -> {statusCode} in {elapseMs}ms from {ipAddress}");
            }
            catch (Exception ex)
            {
                // log error
                stopwatch.Stop();
                _logger.LogError(ex, $"Request: {method} {path} -> {stopwatch.ElapsedMilliseconds}ms from {ipAddress}");

                throw; // Re-throw for Exception Middleware to handle
            }
        }
    }
}
