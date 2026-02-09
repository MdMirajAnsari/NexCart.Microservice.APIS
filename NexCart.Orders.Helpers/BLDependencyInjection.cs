using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NexCart.Orders.Validators;
using FluentValidation;
using NexCart.Orders.ServiceContracts;
using NexCart.Orders.Services;

namespace NexCart.Orders.Helpers
{
    public static class BLDependencyInjection
    {
        public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddValidatorsFromAssemblyContaining<OrderAddRequestValidator>();
            services.AddAutoMapper(typeof(NexCart.Orders.Mappers.OrderToOrderResponseMappingProfile));

            services.AddScoped<IOrdersService, OrdersService>();
            return services;
        }
    }
}
