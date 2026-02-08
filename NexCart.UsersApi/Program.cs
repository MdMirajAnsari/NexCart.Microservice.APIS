using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using NexCart.UsersApi.Middlewares;



var builder = WebApplication.CreateBuilder(args);

// Add Controllers
builder.Services.AddControllers().AddJsonOptions(options =>
{ 
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());  
});

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();

//Add Swagger
builder.Services.AddEndpointsApiExplorer();



//Add Cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(_=>_.SwaggerEndpoint("/swagger/v1/swagger.json","NexCart.UsersApi v1"));
}

app.UseExceptionHandlingMiddleware();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();
