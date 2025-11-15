using System;
using System.IO;
using CalculatorAPI.Resources;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Web;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// Programmatic NLog configuration using ContentRootPath so logs land next to the app content root
try
{
    var logsDir = Path.Combine(builder.Environment.ContentRootPath, "Logs");
    Directory.CreateDirectory(logsDir);

    var nlogConfig = new LoggingConfiguration();

    var consoleTarget = new ConsoleTarget("console")
    {
        Layout = "${longdate} | ${level:uppercase=true} | ${logger} | ${message} ${exception:format=toString}"
    };

    var fileTarget = new FileTarget("file")
    {
        // use ContentRootPath so when running from VS the Logs folder is under the project output
        FileName = Path.Combine(logsDir, "${shortdate}.log"),
        Layout = "${longdate} | ${level:uppercase=true} | ${logger} | ${message} ${exception:format=toString}",
        KeepFileOpen = true,
        ConcurrentWrites = false,
        Encoding = System.Text.Encoding.UTF8
    };

    nlogConfig.AddTarget(consoleTarget);
    nlogConfig.AddTarget(fileTarget);

    // rules: write Info+ to both console and file
    nlogConfig.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, consoleTarget);
    nlogConfig.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, fileTarget);

    LogManager.Configuration = nlogConfig;

    // Direct NLog write to verify target
    var directLogger = LogManager.GetCurrentClassLogger();
    directLogger.Info("DIAGNOSTIC: NLog configured and initial test message");
}
catch (Exception ex)
{
    // If NLog setup fails, write to console for diagnostics
    Console.WriteLine("NLog programmatic configuration failed: " + ex);
}

// Integrate NLog with the Host so Microsoft.Extensions.Logging routes to NLog
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

// Add Swagger/OpenAPI generation (Swashbuckle)
builder.Services.AddSwaggerGen(c =>
{
    // register the schema filter we added (class is in CalculatorAPI.Swagger)

});

// existing helper to add OpenAPI (keeps your previous configuration)
builder.Services.AddOpenApi();
builder.Services.AddScoped<ICalculatorResource, CalculatorResource>();

var app = builder.Build();

// Diagnostic: write direct NLog message after build to ensure file is created
try
{
    var directLogger = LogManager.GetCurrentClassLogger();
    directLogger.Info($"DIAGNOSTIC: App built. Writing test log to {Path.Combine(builder.Environment.ContentRootPath, "Logs")}.");
    Console.WriteLine("Wrote diagnostic NLog info");
}
catch (Exception ex)
{
    Console.WriteLine("Direct NLog write failed: " + ex);
}

// Test logger to validate file logging pipeline (ILogger -> NLog)
try
{
    var testLogger = app.Services.GetRequiredService<Microsoft.Extensions.Logging.ILogger<CalculatorResource>>();
    testLogger.LogInformation("TEST_LOG: NLog pipeline is active at {Time}", DateTime.Now);
}
catch (Exception ex)
{
    Console.WriteLine("Logger test failed: " + ex);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // enable Swagger UI + JSON endpoints in development
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Apply CORS policy so preflight and cross-origin requests from Angular are allowed
app.UseCors("AllowAngularDev");

app.UseAuthorization();

app.MapControllers();

app.Run();

// Ensure NLog flushes and stops on shutdown
NLog.LogManager.Shutdown();