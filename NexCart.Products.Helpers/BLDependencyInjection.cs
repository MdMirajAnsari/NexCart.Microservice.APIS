using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NexCart.Products.DTO;
using NexCart.Products.Mappers;
using NexCart.Products.ServiceContracts;
using NexCart.Products.Services;
using NexCart.Products.Validators;
using RabbitMQ;


namespace NexCart.Products.Helpers
{
    public static class BLDependencyInjection
    {
        public static IServiceCollection AddDataBusinessLogicLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => { }, typeof(ProductAddRequestToProductMappingProfile).Assembly);
            services.AddValidatorsFromAssemblyContaining<ProductAddRequestValidator>();
            //services.AddTransient<IValidator<ProductAddRequest>, ProductAddRequestValidator>();

            services.AddScoped<IProductsService, ProductsService>();
            services.AddTransient<IRabbitMQPublisher, RabbitMQPublisher>();
            return services;
        }
    }
}
