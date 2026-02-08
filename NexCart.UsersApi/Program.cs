using FluentValidation.AspNetCore;
using NexCart.Users.Infrastructure;
using NexCart.Users.ServiceContracts;
using NexCart.UsersApi.Middlewares;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Controllers + JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

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
