using FluentValidation;
using SmartHire.Application.Common.Models;
using System.Text.Json;

namespace SmartHire.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.LogError(
                ex,
                "Unhandled exception occurred. Path: {Path}, Method: {Method}",
                context.Request.Path,
                context.Request.Method
            );

            if (ex is ValidationException validationException)
            {
                var errors = validationException.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => x.ErrorMessage).ToArray()
                    );

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                var response = new
                {
                    success = false,
                    errors
                };

                await context.Response.WriteAsJsonAsync(response);

                return;
            }

            var error = Error.Internal(
                "An unexpected error occurred. Please try again later.");

            var result = Result.Failure(error);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode =
                StatusCodes.Status500InternalServerError;

            var jsonResponse = JsonSerializer.Serialize(
                result,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }
            );

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
