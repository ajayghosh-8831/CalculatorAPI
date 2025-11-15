using CalculatorAPI.Resources;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// NOTE: file-based logging disabled to avoid startup I/O overhead during debugging.

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

app.Run();app.Run();