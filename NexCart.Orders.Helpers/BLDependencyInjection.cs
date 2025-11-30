using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NexCart.Orders.Validators;
using FluentValidation;


namespace NexCart.Orders.Helpers
{
    public static class BLDependencyInjection
    {
        public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddValidatorsFromAssemblyContaining<OrderAddRequestValidator>();
            // TODO: Uncomment when OrderAddRequestToOrderMappingProfile is implemented
            // services.AddAutoMapper(typeof(OrderAddRequestToOrderMappingProfile).Assembly);
            // TODO: Uncomment when IOrdersService and OrdersService are implemented
            // services.AddScoped<IOrdersService, OrdersService>();
            return services;
        }
    }
}
