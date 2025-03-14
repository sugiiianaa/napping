using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Napping.Application;
using Napping.Infrastructure;
using Napping.Infrastructure.Data;
var builder = WebApplication.CreateBuilder(args);

ConfigureConfiguration(builder);
ConfigureLogging(builder);
ConfigureServices(builder);

var app = builder.Build();

ConfigureMiddleware(app);
await ApplyDatabaseMigrationsAsync(app);
ConfigureApplicationLifecycleLogging(app);

app.Run();

void ConfigureConfiguration(WebApplicationBuilder webBuilder)
{
    webBuilder.Configuration
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{webBuilder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables();

    if (webBuilder.Environment.IsDevelopment())
    {
        webBuilder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);
    }
}

void ConfigureLogging(WebApplicationBuilder webBuilder)
{
    webBuilder.Logging.ClearProviders();
    webBuilder.Logging.AddConsole();
    webBuilder.Logging.SetMinimumLevel(LogLevel.Information);
}

void ConfigureServices(WebApplicationBuilder webBuilder)
{
    webBuilder.Services.AddAllElasticApm();
    webBuilder.Services.AddProblemDetails();
    webBuilder.Services.AddControllers();
    webBuilder.Services.AddEndpointsApiExplorer();
    webBuilder.Services.AddSwaggerGen();

    webBuilder.Services
        .AddApplicationLayer()
        .AddInfrastructureLayer();

    webBuilder.Services.AddDbContext<AppDbContext>((services, options) =>
    {
        var connectionString = Environment.GetEnvironmentVariable("NAPPING_CONNECTION_STRING")
                            ?? webBuilder.Configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException(
                "The connection string was not found in environment variables or configuration.");
        }

        options.UseNpgsql(connectionString);
    });
}

void ConfigureMiddleware(WebApplication webApp)
{
    webApp.UseExceptionHandler(exceptionHandlerApp =>
    {
        exceptionHandlerApp.Run(HandleException);
    });

    if (webApp.Environment.IsDevelopment())
    {
        webApp.UseSwagger();
        webApp.UseSwaggerUI();
    }

    webApp.UseHttpsRedirection();
    webApp.UseAuthorization();
    webApp.MapControllers();
}

async Task ApplyDatabaseMigrationsAsync(WebApplication webApp)
{
    using var scope = webApp.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    logger.LogInformation("Checking for pending migrations...");

    var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
    if (pendingMigrations.Any())
    {
        logger.LogInformation("Applying {MigrationCount} migrations...", pendingMigrations.Count());
        foreach (var migration in pendingMigrations)
        {
            logger.LogInformation("Applying migration: {MigrationName}", migration);
        }

        await dbContext.Database.MigrateAsync();
        logger.LogInformation("All migrations applied successfully");
    }
    else
    {
        logger.LogInformation("No pending migrations found");
    }
}

async Task HandleException(HttpContext context)
{
    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

    if (exception is ValidationException validationException)
    {
        logger.LogError("Validation error occurred: {ErrorMessage}", validationException.Message);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation Error",
            Detail = "One or more validation errors occurred."
        };

        problemDetails.Extensions["errors"] = validationException.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}

void ConfigureApplicationLifecycleLogging(WebApplication webApp)
{
    var logger = webApp.Services.GetRequiredService<ILogger<Program>>();

    webApp.Lifetime.ApplicationStarted.Register(() =>
    {
        logger.LogInformation("Server is ready and listening on the following URLs:");
        foreach (var address in webApp.Urls)
        {
            logger.LogInformation("- {Address}", address);
            if (address.Contains("localhost"))
            {
                logger.LogInformation("Swagger UI available at: {SwaggerUrl}", $"{address}/swagger");
            }
        }
    });
}