using NexCart.Orders.Helpers;
using Microsoft.AspNetCore.Builder;

using NLog;
using NLog.Web;

var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
try
{
    logger.Debug("init main");
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Register data access and business logic for Orders service
    builder.Services.AddDataAccessLayer(builder.Configuration);
    builder.Services.AddBusinessLogicLayer(builder.Configuration);

    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();
    logger.Info("NexCart.OrdersApi starting");

    app.UseMiddleware<NexCart.OrdersApi.Middlewares.RequestLoggingMiddleware>();
    app.UseRouting();

    // Enable Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NexCart.OrdersApi v1"));

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
