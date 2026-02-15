using Microsoft.OpenApi;
using NexCart.Orders.Helpers;
using NLog;
using NLog.Web;

var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

try
{
    logger.Debug("init main");

    var builder = WebApplication.CreateBuilder(args);

    // Logging
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Register services
    builder.Services.AddDataAccessLayer(builder.Configuration);
    builder.Services.AddBusinessLogicLayer(builder.Configuration);

    builder.Services.AddControllers();

    // Swagger services
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "NexCart Orders API",
            Version = "v1",
            Description = "Orders microservice for NexCart"
        });
    });

    var app = builder.Build();

    logger.Info("NexCart.OrdersApi starting");

    // Swagger MUST be first middleware
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "NexCart Orders API v1");
        options.RoutePrefix = "swagger";   // open at /swagger
    });

    // Optional: test endpoint
    app.MapGet("/health", () => "Orders API running");

    // Routing
    app.UseRouting();

    // ⚠️ Add custom middleware AFTER swagger
    // If this breaks Swagger, comment it and fix middleware stream handling
    app.UseMiddleware<NexCart.OrdersApi.Middlewares.RequestLoggingMiddleware>();

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
    LogManager.Shutdown();
}
