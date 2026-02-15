using FluentValidation.AspNetCore;
using Microsoft.OpenApi;
using NexCart.Products.Helpers;
using NexCart.ProductsApi.Middleware;
using NLog.Web;
using System.Text.Json.Serialization;

var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

try
{
    logger.Debug("init main");
    var builder = WebApplication.CreateBuilder(args);

    // ADD DAL AND BAL SERVICES
    builder.Services.AddDataAccessLayer(builder.Configuration);
    builder.Services.AddDataBusinessLogicLayer();

    builder.Services.AddControllers();

    // FLUENT VALIDATION
    builder.Services.AddFluentValidationAutoValidation();

    // json enum converter
    builder.Services.ConfigureHttpJsonOptions(options =>
    {
        options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

    // Swagger/OpenAPI
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "NexCart Products API",
            Version = "v1"
        });
    });

    var app = builder.Build();

    logger.Info("NexCart.ProductsApi starting");

    // Swagger FIRST (important)
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "NexCart.ProductsApi v1"));

    app.UseExceptionHandlingMiddleware();
    app.UseMiddleware<RequestLoggingMiddleware>();

    app.UseRouting();

    // Authorization
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}
