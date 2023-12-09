using Microsoft.AspNetCore.Cors.Infrastructure;
using SmartHouseHub.DAL.Interfaces;
using SmartHouseHub.DAL.Repositories;
using SmartHouseHub.Domain.Identity;
using SmartHouseHub.Service.Implementations;
using SmartHouseHub.Service.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SmartHouseHub
{
    public static class Initializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<User>, UserRepository>();
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenProvider, TokenProvider>();
        }

    }
}
