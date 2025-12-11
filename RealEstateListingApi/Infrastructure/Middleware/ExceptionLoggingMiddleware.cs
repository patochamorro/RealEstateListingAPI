using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RealEstateListingApi.Application.Exceptions;

namespace RealEstateListingApi.Infrastructure.Middleware
{
    public class ExceptionLoggingMiddleware
    {
        private const string ServiceName = "RealEstateListingApi";
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionLoggingMiddleware> _logger;

        public ExceptionLoggingMiddleware(RequestDelegate next, ILogger<ExceptionLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = Guid.NewGuid();
            var endpointName = context.GetEndpoint()?.DisplayName ?? $"{context.Request.Method} {context.Request.Path}";
            try
            {
                await _next(context);

                var status = context.Response.StatusCode >= 400 ? "FAILED" : "OK";
                var message = status == "OK"
                    ? string.Empty
                    : $"Request ended with status code {context.Response.StatusCode}.";

                _logger.LogInformation("{Service}|{Endpoint}|{CorrelationId}|{Status}|{Message}|{StackTrace}", ServiceName, endpointName, correlationId, status, message, string.Empty);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, correlationId, endpointName);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, Guid correlationId, string endpointName)
        {
            var serviceException = exception as ServiceException;
            var severity = serviceException?.Severity ?? ServiceExceptionSeverity.Technical;
            var statusCode = severity == ServiceExceptionSeverity.Business
                ? StatusCodes.Status400BadRequest
                : StatusCodes.Status500InternalServerError;

            var message = serviceException is not null
                ? $"[{serviceException.Component}] {serviceException.Message}"
                : exception.Message;
            var stackTrace = exception.StackTrace ?? string.Empty;
            var status = "FAILED";

            _logger.LogError("{Service}|{Endpoint}|{CorrelationId}|{Status}|{Message}|{StackTrace}", ServiceName, endpointName, correlationId, status, message, stackTrace);

            if (context.Response.HasStarted)
            {
                return;
            }

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var payload = JsonSerializer.Serialize(new
            {
                correlationId,
                status,
                severity = severity.ToString(),
                message
            });

            await context.Response.WriteAsync(payload);
        }
    }
}
