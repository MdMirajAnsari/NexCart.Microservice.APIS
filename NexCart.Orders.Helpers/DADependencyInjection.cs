using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NexCart.Orders.Repositories;
using NexCart.Orders.RepositoryContracts;

namespace NexCart.Orders.Helpers
{
    public static class DADependencyInjection
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrdersDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IOrdersRepository, OrdersRepository>();
            return services;
        }
    }
}
