using EpicMarket.Business.API.Helpers;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Services;

namespace EpicMarket.Business.API.Extension
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
             services.AddDbContext<ApplicationDbContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<LogUserActivity>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            return services;
        }
    }
}
