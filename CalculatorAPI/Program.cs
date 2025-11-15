using CalculatorAPI.Logging;
using CalculatorAPI.Resources;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// configure file logger path
var logFilePath = Path.Combine(builder.Environment.ContentRootPath, "Logs", "app.log");
var fileLoggerProvider = new FileLoggerProvider(logFilePath);

builder.Logging.ClearProviders();
builder.Logging.AddProvider(fileLoggerProvider);
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);

// Add CORS policy for Angular dev server
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // enable Swagger UI + JSON endpoints in development
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();