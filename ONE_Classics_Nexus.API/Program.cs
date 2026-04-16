using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Application.Abstractions.Services;
using ONE_Classics_Nexus.Application.Services;
using ONE_Classics_Nexus.Infrastructure;
using ONE_Classics_Nexus.Infrastructure.Database;
using ONE_Classics_Nexus.Infrastructure.DependencyInjection;
using ONE_Classics_Nexus.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Core framework services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();

// Nexus registrations
builder.Services.AddNexusInfrastructure();
builder.Services.AddValuationServices();

// Repository / service registrations
builder.Services.AddScoped<IDbConnectionFactory, MariaDbConnectionFactory>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IAppraisalRepository, AppraisalRepository>();
builder.Services.AddScoped<IInspectionRepository, InspectionRepository>();

// Uncomment when implemented
// builder.Services.AddScoped<IComparableRepository, ComparableRepository>();

var app = builder.Build();

// Global exception handling
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
        var exception = exceptionFeature?.Error;

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Title = "Server error",
            Status = StatusCodes.Status500InternalServerError,
            Detail = exception?.Message,
            Instance = context.Request.Path
        };

        await context.Response.WriteAsJsonAsync(problem);
    });
});

// HTTP pipeline
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Testing"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
public partial class Program { }