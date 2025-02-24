using Infrastructure.Redis;
using Infrastructure.RelationalDatabase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UseCase.RelationalDatabase;
using UseCase.Shared.Interfaces;

namespace Infrastructure
{
    public static class Configuration
    {
        public static IServiceCollection AddInfrastructureConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IConfiguration>(configuration);
            services.AddTransient<DiplomaBdContext, MsSqlDiplomaBdContext>();
            services.AddTransient<IRedisService, RedisService>();

            return services;
        }
    }
}
