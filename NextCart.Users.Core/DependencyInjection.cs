using Microsoft.Extensions.DependencyInjection;
using NexCart.Users.ServiceContracts;
using NexCart.Users.Services;
using NexCart.Users.Validators;
using FluentValidation;
using NexCart.Users.DTO;

namespace NextCart.Users.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IValidator<LoginRequest>, LoginRequestValidator>();
            return services;
        }
    }
}
