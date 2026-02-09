using NexCart.Orders.Helpers;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Register data access and business logic for Orders service
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBusinessLogicLayer(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseRouting();

// Enable Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NexCart.OrdersApi v1"));

app.UseAuthorization();

app.MapControllers();

app.Run();
