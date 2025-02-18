using Infrastructure.RelationalDatabase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UseCase.RelationalDatabase;

namespace Infrastructure
{
    public static class Configuration
    {
        public static IServiceCollection InfrastructureConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IConfiguration>(configuration);
            services.AddTransient<DiplomaBdContext, MsSqlDiplomaBdContext>();
            return services;
        }
    }
}
