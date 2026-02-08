using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NexCart.Users.Infrastructure.Repositories;
using NexCart.Users.RepositoryContracts;
using NexCart.Users.ServiceContracts;
using NexCart.Users.Services;

namespace NexCart.Users.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUsersRepository, UserRepository>();
        services.AddScoped<IUsersService, UsersService>();
        return services;
    }
}