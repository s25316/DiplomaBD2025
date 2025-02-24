using Microsoft.Extensions.DependencyInjection;

namespace Domain
{
    public static class Configuration
    {
        public static IServiceCollection AddDomainConfiguration(this IServiceCollection services)
        {
            return services;
        }
    }
}
