using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Hosting;
using ONE_Classics_Nexus.API.Middleware;
using ONE_Classics_Nexus.Application.Exceptions;
using Xunit;

namespace ONE_Classics_Nexus.Tests.Unit.Middleware;

public sealed class GlobalExceptionHandlingMiddlewareTests
{
    private sealed class TestHostEnvironment : IHostEnvironment
    {
        public string EnvironmentName { get; set; }
        public string ApplicationName { get; set; } = string.Empty;
        public string ContentRootPath { get; set; } = string.Empty;

        public TestHostEnvironment(string environmentName) => EnvironmentName = environmentName;
    }

    [Fact]
    public async Task InvokeAsync_WhenDownstreamThrowsConflictException_InProduction_Returns409AndTraceCorrelation()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Path = "/api/test";
        context.Response.Body = new MemoryStream();

        RequestDelegate next = _ => Task.FromException(new ConflictException("duplicate vin"));

        var logger = NullLogger<GlobalExceptionHandlingMiddleware>.Instance;
        var env = new TestHostEnvironment("Production");

        var middleware = new GlobalExceptionHandlingMiddleware(next, logger, env);

        // Act
        await middleware.InvokeAsync(context);

        // Assert basic response
        context.Response.StatusCode.Should().Be(409);
        context.Response.ContentType.Should().Be("application/problem+json");

        // Read body
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        using var sr = new StreamReader(context.Response.Body);
        var body = await sr.ReadToEndAsync();

        // Deserialize ProblemDetails for structured assertions
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var problem = JsonSerializer.Deserialize<ProblemDetails>(body, options);

        problem.Should().NotBeNull();
        problem!.Status.Should().Be(409);
        problem.Title.Should().Contain("Conflict");

        // Check that traceId and correlationId are present at top-level JSON
        using var doc = JsonDocument.Parse(body);
        var root = doc.RootElement;
        root.TryGetProperty("traceId", out var traceEl).Should().BeTrue();
        traceEl.GetString().Should().NotBeNullOrWhiteSpace();

        root.TryGetProperty("correlationId", out var corrEl).Should().BeTrue();
        corrEl.GetString().Should().NotBeNullOrWhiteSpace();

        // In Production detail should be the exception message (short)
        root.TryGetProperty("detail", out var detailEl).Should().BeTrue();
        detailEl.GetString().Should().Contain("duplicate vin");
    }

    [Fact]
    public async Task InvokeAsync_WhenDownstreamThrowsConflictException_InDevelopment_Returns409WithFullDetail()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Path = "/api/test";
        context.Response.Body = new MemoryStream();

        RequestDelegate next = _ => Task.FromException(new ConflictException("duplicate vin"));

        var logger = NullLogger<GlobalExceptionHandlingMiddleware>.Instance;
        var env = new TestHostEnvironment("Development");

        var middleware = new GlobalExceptionHandlingMiddleware(next, logger, env);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(409);
        context.Response.ContentType.Should().Be("application/problem+json");

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        using var sr = new StreamReader(context.Response.Body);
        var body = await sr.ReadToEndAsync();

        using var doc = JsonDocument.Parse(body);
        var root = doc.RootElement;

        // Ensure trace/correlation present
        root.TryGetProperty("traceId", out var traceEl).Should().BeTrue();
        traceEl.GetString().Should().NotBeNullOrWhiteSpace();

        root.TryGetProperty("correlationId", out var corrEl).Should().BeTrue();
        corrEl.GetString().Should().NotBeNullOrWhiteSpace();

        // In Development the middleware returns ex.ToString() in 'detail' (type + message + stack)
        root.TryGetProperty("detail", out var detailEl).Should().BeTrue();
        var detail = detailEl.GetString() ?? string.Empty;
        detail.Should().Contain("ConflictException");
        detail.Should().Contain("duplicate vin");
        // stack trace lines typically include " at ", assert that too to confirm full detail
        detail.Should().Contain(" at ");
    }
}