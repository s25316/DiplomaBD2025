using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Domain
{
    public static class Configuration
    {
        public static IServiceCollection DomainConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IConfiguration>(configuration);
            return services;
        }
    }
}
