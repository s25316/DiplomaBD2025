// Ignore Spelling: Jwt, Redis, Sql
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using UseCase.Roles.Guests.Queries.Dictionaries.Repositories;
using UseCase.Shared.Exceptions;
using UseCase.Shared.Repositories.Addresses;
using UseCase.Shared.Services.Authentication.Generators;
using UseCase.Shared.Services.Authentication.Inspectors;
using UseCase.Shared.Services.Time;

namespace UseCase
{
    public static class Configuration
    {
        // Properties
        public static string RedisConnectionString { get; private set; } = null!;
        public static string KafkaConnectionString { get; private set; } = null!;
        public static string RelationalDatabaseConnectionString { get; private set; } = null!;

        public static string JwtIssuer { get; private set; } = null!;
        public static string JwtAudience { get; private set; } = null!;
        public static string JwtSecret { get; private set; } = null!;


        // Methods
        public static IServiceCollection AddCheckUserSecrets(
          this IServiceCollection services,
          IConfiguration configuration)
        {
            RedisConnectionString = configuration.GetSection("ConnectionStrings")["Redis"] ??
                throw new UseCaseLayerException(Messages.UserSecrets_NotFound_Redis);
            KafkaConnectionString = configuration.GetSection("ConnectionStrings")["Kafka"] ??
                throw new UseCaseLayerException(Messages.UserSecrets_NotFound_Kafka);
            RelationalDatabaseConnectionString = configuration.GetSection("ConnectionStrings")["RelationalDatabase"] ??
                throw new UseCaseLayerException(Messages.UserSecrets_NotFound_RelationalDatabase);

            JwtIssuer = configuration.GetSection("Authentication")["Issuer"] ??
                throw new UseCaseLayerException(Messages.UserSecrets_NotFound_Issuer);
            JwtAudience = configuration.GetSection("Authentication")["Audience"] ??
                throw new UseCaseLayerException(Messages.UserSecrets_NotFound_Audience);
            JwtSecret = configuration.GetSection("Authentication")["Secret"] ??
                throw new UseCaseLayerException(Messages.UserSecrets_NotFound_Secret);

            return services;
        }

        public static IServiceCollection AddUseCaseConfiguration(this IServiceCollection services)
        {
            services.AddMediatR(config => config.RegisterServicesFromAssembly(
                Assembly.GetExecutingAssembly()
                ));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Guest
            services.AddTransient<IDictionariesRepository, DictionariesRepository>();

            // Shared
            services.AddTransient<IAddressRepository, AddressRepository>();

            services.AddTransient<IAuthenticationGeneratorService, AuthenticationGeneratorService>();
            services.AddTransient<IAuthenticationInspectorService, AuthenticationInspectorService>();
            services.AddTransient<ITimeService, TimeService>();
            return services;
        }
    }
}
