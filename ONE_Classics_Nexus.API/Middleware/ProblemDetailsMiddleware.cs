using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ONE_Classics_Nexus.Application.Exceptions;

namespace ONE_Classics_Nexus.API.Middleware;

public sealed class ProblemDetailsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ProblemDetailsMiddleware> _logger;

    public ProblemDetailsMiddleware(RequestDelegate next, ILogger<ProblemDetailsMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception for request {Method} {Path}", context.Request.Method, context.Request.Path);
            await WriteProblemDetailsAsync(context, ex);
        }
    }

    private static async Task WriteProblemDetailsAsync(HttpContext context, Exception ex)
    {
        var (status, title) = ex switch
        {
            ValidationException => (StatusCodes.Status400BadRequest, "Validation error"),
            NotFoundException => (StatusCodes.Status404NotFound, "Resource not found"),
            ConflictException => (StatusCodes.Status409Conflict, "Conflict"),
            _ => (StatusCodes.Status500InternalServerError, "Server error")
        };

        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = ex.Message,
            Instance = context.Request.Path
        };

        context.Response.StatusCode = status;
        context.Response.ContentType = "application/problem+json";

        var json = JsonSerializer.Serialize(problem);
        await context.Response.WriteAsync(json);
    }
}