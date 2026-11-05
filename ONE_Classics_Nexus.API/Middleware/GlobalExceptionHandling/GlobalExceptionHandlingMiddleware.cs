using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using ONE_Classics_Nexus.Application.Exceptions;
using System.Diagnostics;
using System.Text.Json;

namespace ONE_Classics_Nexus.API.Middleware.GlobalExceptionHandling;

public sealed class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _env;
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    public GlobalExceptionHandlingMiddleware(RequestDelegate next,
                                             ILogger<GlobalExceptionHandlingMiddleware> logger,
                                             IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Response.HasStarted)
        {
            await _next(context);
            return;
        }

        var correlationId = GetOrCreateCorrelationId(context);
        using (_logger.BeginScope(new Dictionary<string, object> { ["CorrelationId"] = correlationId }))
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception for request {Method} {Path}", context.Request.Method, context.Request.Path);
                await HandleExceptionAsync(context, ex, correlationId);
            }
        }
    }

    private static string GetOrCreateCorrelationId(HttpContext context)
    {
        const string header = "X-Correlation-ID";
        if (context.Request.Headers.TryGetValue(header, out var existing) && !StringValues.IsNullOrEmpty(existing))
        {
            context.Response.Headers[header] = existing.ToString();
            return existing.ToString();
        }

        var newId = Activity.Current?.Id ?? context.TraceIdentifier;
        context.Response.Headers[header] = newId;
        return newId;
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex, string correlationId)
    {
        if (context.Response.HasStarted)
        {
            _logger.LogWarning("Response has already started, cannot write error response. CorrelationId: {CorrelationId}", correlationId);
            return;
        }

        (int Status, string Title, object? Additional) = ex switch
        {
            ValidationException ve => (StatusCodes.Status400BadRequest, "Validation error", ExtractValidationErrors(ve)),
            ArgumentNullException => (StatusCodes.Status400BadRequest, "Bad Request", null),
            ArgumentException => (StatusCodes.Status400BadRequest, "Bad Request", null),
            NotFoundException => (StatusCodes.Status404NotFound, "Resource not found", null),
            Exception e when IsConflictException(e) => (StatusCodes.Status409Conflict, "Conflict", null),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized", null),
            _ => (StatusCodes.Status500InternalServerError, "Server error", null)
        };

        var traceId = Activity.Current?.Id ?? context.TraceIdentifier;

        var problem = new ProblemDetails
        {
            Type = Status switch
            {
                StatusCodes.Status400BadRequest => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                StatusCodes.Status401Unauthorized => "https://tools.ietf.org/html/rfc7235#section-3.1",
                StatusCodes.Status404NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                StatusCodes.Status409Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                _ => "about:blank"
            },
            Title = Title,
            Status = Status,
            Detail = _env.IsDevelopment() ? ex.ToString() : ex.Message,
            Instance = context.Request.Path
        };

        problem.Extensions["traceId"] = traceId;
        problem.Extensions["correlationId"] = correlationId;
        if (Additional is not null) problem.Extensions["errors"] = Additional;

        context.Response.Clear();
        context.Response.StatusCode = Status;
        context.Response.ContentType = "application/problem+json";

        var json = JsonSerializer.Serialize(problem, SerializerOptions);
        await context.Response.WriteAsync(json);
    }

    private static bool IsConflictException(Exception ex)
    {
        for (Exception? current = ex; current is not null; current = current.InnerException)
        {
            // Exact type handled in this middleware namespace
            if (current is ConflictException)
            {
                return true;
            }

            // Handles ConflictException defined in a different namespace/assembly
            if (string.Equals(current.GetType().Name, nameof(ConflictException), StringComparison.Ordinal))
            {
                return true;
            }

            // Handles DB duplicate-key failures surfaced as provider exceptions
            var message = current.Message;
            if (message.Contains("Duplicate entry", StringComparison.OrdinalIgnoreCase)
                || message.Contains("duplicate key", StringComparison.OrdinalIgnoreCase)
                || message.Contains("UNIQUE constraint", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private static object? ExtractValidationErrors(ValidationException ve)
    {
        var errorsProp = ve.GetType().GetProperty("Errors");
        if (errorsProp is not null)
        {
            var value = errorsProp.GetValue(ve);
            if (value is not null) return value;
        }

        // Fallback to Message if no structured data
        return new[] { ve.Message };
    }
}

public static class GlobalExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
    }
}

// If you do not have this interface, you need to define it as below (temporary stub for compilation):
public interface IJsonRpcClient
{
    Task<TResult> InvokeAsync<TResult>(string method, object? arg, CancellationToken cancellationToken);
}