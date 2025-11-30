using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NexCart.Products.Context;
using NexCart.Products.Repositories;
using NexCart.Products.RepositoryContracts;


namespace NexCart.Products.Helpers
{
    public static class DADependencyInjection
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            //string connectionStringTemplate = configuration.GetConnectionString("DefaultConnection")!;

            //string connectionString = connectionStringTemplate.Replace("$MYSQL_HOST", Environment.GetEnvironmentVariable("MYSQL_HOST"))
            //.Replace("$MYSQL_PASSWORD", Environment.GetEnvironmentVariable("MYSQL_PASSWORD"))
            //.Replace("$MYSQL_USER", Environment.GetEnvironmentVariable("MYSQL_USER"))
            //.Replace("$MYSQL_DATABASE", Environment.GetEnvironmentVariable("MYSQL_DATABASE"))
            //.Replace("$MYSQL_PORT", Environment.GetEnvironmentVariable("MYSQL_PORT"));

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IProductsRepository, ProductsRepository>();
            return services;
        }
    }
}
