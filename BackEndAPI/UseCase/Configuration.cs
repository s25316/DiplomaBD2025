using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace UseCase
{
    public static class Configuration
    {
        public static IServiceCollection UseCaseConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IConfiguration>(configuration);
            services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            return services;
        }
    }
}
