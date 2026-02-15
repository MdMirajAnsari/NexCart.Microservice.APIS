using FluentValidation.AspNetCore;
using NexCart.Users.Infrastructure;
using NexCart.Users.ServiceContracts;
using NexCart.UsersApi.Middlewares;
using System.Text.Json.Serialization;
using NLog;
using NLog.Web;

var logger = NLog.LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();
try
{
    logger.Debug("init main");
    var builder = WebApplication.CreateBuilder(args);

    // Configure logging for NLog
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Controllers + JSON
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(
                new JsonStringEnumConverter());
        });

// FluentValidation
    // FluentValidation
    builder.Services.AddFluentValidationAutoValidation();

// Swagger ✅
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // 🔥 REQUIRED

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


builder.Services.AddInfrastructure(builder.Configuration);
    var app = builder.Build();

    // HTTP pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "NexCart.UsersApi v1"));
    }

    // Custom exception middleware
    app.UseExceptionHandlingMiddleware();

    app.UseRouting();

    // ⚠️ Only keep these if authentication is configured
    // app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    // NLog: catch setup errors
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}
