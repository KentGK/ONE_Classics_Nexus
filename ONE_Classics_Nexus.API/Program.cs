// Import custom middleware for centralized error handling
using ONE_Classics_Nexus.API.Middleware;

// Import Infrastructure DI extension (registers DB, repositories, handlers)
using ONE_Classics_Nexus.Infrastructure.DependencyInjection;

// Create the WebApplication builder (bootstraps configuration, DI container, logging)
var builder = WebApplication.CreateBuilder(args);


// ---------------------------------------------------------
// SERVICE REGISTRATION (Dependency Injection Container)
// ---------------------------------------------------------

// Adds MVC controller support (enables [ApiController], routing, model binding)
builder.Services.AddControllers();

// Enables endpoint discovery for Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();

// Registers Swagger generator (creates OpenAPI spec + UI)
builder.Services.AddSwaggerGen();

// Registers ALL Infrastructure dependencies:
// - MariaDbConnectionFactory
// - Repositories (Vehicle, Appraisal, Inspection)
// - Application Handlers (CreateVehicle, etc.)
builder.Services.AddNexusInfrastructure();


// ---------------------------------------------------------
// BUILD APPLICATION PIPELINE
// ---------------------------------------------------------

// Build the application (finalizes DI container + middleware pipeline)
var app = builder.Build();


// ---------------------------------------------------------
// MIDDLEWARE PIPELINE (Request Processing Order Matters)
// ---------------------------------------------------------

// Enable Swagger JSON endpoint
app.UseSwagger();

// Enable Swagger UI (interactive testing interface at /swagger)
app.UseSwaggerUI();

// Custom middleware to convert exceptions into ProblemDetails responses
// Handles:
//   - ValidationException → 400
//   - NotFoundException → 404
//   - ConflictException → 409
//   - All others → 500
app.UseMiddleware<ProblemDetailsMiddleware>();


// ---------------------------------------------------------
// ENDPOINT MAPPING
// ---------------------------------------------------------

// Maps controller routes (e.g., /api/vehicles, /api/appraisals, /api/inspections)
app.MapControllers();


// ---------------------------------------------------------
// APPLICATION START
// ---------------------------------------------------------

// Starts the web server (Kestrel) and begins listening for HTTP requests
app.Run();