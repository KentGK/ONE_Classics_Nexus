using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using ONE_Classics_Nexus.API.Middleware.GlobalExceptionHandling;
using ONE_Classics_Nexus.Application.Exceptions;
using System.Text.Json;

namespace ONE_Classics_Nexus.Tests.Unit.Middleware;

public sealed class GlobalExceptionHandlingMiddlewareTests
{
    private sealed class TestHostEnvironment : IHostEnvironment
    {
        public string EnvironmentName { get; set; }
        public string ApplicationName { get; set; } = string.Empty;
        public string ContentRootPath { get; set; } = string.Empty;
        public Microsoft.Extensions.FileProviders.IFileProvider ContentRootFileProvider { get; set; } =
            new Microsoft.Extensions.FileProviders.PhysicalFileProvider(System.IO.Directory.GetCurrentDirectory());

        public TestHostEnvironment(string environmentName) => EnvironmentName = environmentName;
    }

    [Fact]
    public async Task InvokeAsync_WhenDownstreamThrowsConflictException_Returns409ProblemDetails()
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

        // Assert
        context.Response.StatusCode.Should().Be(409);
        context.Response.ContentType.Should().Be("application/problem+json");

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        using var sr = new StreamReader(context.Response.Body);
        var body = await sr.ReadToEndAsync();

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var problem = JsonSerializer.Deserialize<ProblemDetails>(body, options);

        problem.Should().NotBeNull();
        problem!.Status.Should().Be(409);
        problem.Title.Should().Contain("Conflict");
    }
}
