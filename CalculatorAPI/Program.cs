using System;
using System.IO;
using CalculatorAPI.Resources;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// Set NLog variable to point to project source directory for logs
var projectRoot = builder.Environment.ContentRootPath;
var logsPath = Path.Combine(projectRoot, "Logs");
NLog.GlobalDiagnosticsContext.Set("logDirectory", logsPath);

// NLog will automatically load nlog.config from the application's base directory
// Clear default providers and use NLog
builder.Logging.ClearProviders();
builder.Host.UseNLog();

// Add services to the container.

// Add CORS policy for Angular dev server (temporarily allow all origins for debugging)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});
builder.Services.AddControllers();

// existing helper to add OpenAPI (keeps your previous configuration)
builder.Services.AddOpenApi();
builder.Services.AddScoped<ICalculatorResource, CalculatorResource>();

var app = builder.Build();

// Log application startup
try
{
    var testLogger = app.Services.GetRequiredService<Microsoft.Extensions.Logging.ILogger<Program>>();
    testLogger.LogInformation("Application started successfully at {Time}. Logs directory: {LogsPath}", DateTime.Now, logsPath);
}
catch (Exception ex)
{
    Console.WriteLine("Logger test failed: " + ex);
}

app.UseHttpsRedirection();

// Apply CORS policy so preflight and cross-origin requests from Angular are allowed
app.UseCors("AllowAngularDev");

app.UseAuthorization();

app.MapControllers();

app.Run();

// Ensure NLog flushes and stops on shutdown
NLog.LogManager.Shutdown();