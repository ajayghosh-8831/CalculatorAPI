using CalculatorAPI.Resources;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// Set NLog variable to point to project source directory for logs
var projectRoot = builder.Environment.ContentRootPath;
var logsPath = Path.Combine(projectRoot, "Logs");
NLog.GlobalDiagnosticsContext.Set("logDirectory", logsPath);
builder.Logging.ClearProviders();
builder.Host.UseNLog();

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
builder.Services.AddOpenApi();
builder.Services.AddScoped<ICalculatorResource, CalculatorResource>();

var app = builder.Build();

// Log application startup
try
{
    var initialLogger = app.Services.GetRequiredService<Microsoft.Extensions.Logging.ILogger<Program>>();
    initialLogger.LogInformation("Application started successfully at {Time}. Logs directory: {LogsPath}", DateTime.Now, logsPath);
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