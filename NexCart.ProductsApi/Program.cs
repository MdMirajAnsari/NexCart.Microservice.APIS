using NexCart.ProductsApi.Middleware;
using NexCart.Products.Helpers;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using NLog;
using NLog.Web;

var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
try
{
    logger.Debug("init main");
    var builder = WebApplication.CreateBuilder(args);

//ADD DAL AND BAL SERVICES
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddDataBusinessLogicLayer();

builder.Services.AddControllers();

//FLUENT VALIDATION
builder.Services.AddFluentValidationAutoValidation();

//json to enum
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

    var app = builder.Build();

    logger.Info("NexCart.ProductsApi starting");

    app.UseExceptionHandlingMiddleware();
    app.UseMiddleware<NexCart.ProductsApi.Middleware.RequestLoggingMiddleware>();
    app.UseRouting();

// Swagger middleware — enable in all environments so UI is reachable when launched
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NexCart.ProductsApi v1"));

//Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
