using Confluent.Kafka;
using Domain;
using Infrastructure;
using StackExchange.Redis;
using UseCase;

namespace BackEndAPI
{
    public static class Configuration
    {
        public static IServiceCollection AddCheckUserSecrets(
          this IServiceCollection services,
          IConfiguration configuration)
        {
            return services;
        }

        //Add mwthod Like config only for dependencies
        public static IServiceCollection AddDependencies(
          this IServiceCollection services,
          IConfiguration configuration)
        {
            //Layers Dependencies
            services.DomainConfiguration(configuration);
            services.UseCaseConfiguration(configuration);
            services.InfrastructureConfiguration(configuration);

            //Get ConnectionStrings Section
            var connectionStrings = configuration.GetSection("ConnectionStrings");
            var redis = connectionStrings["Redis"] ?? throw new Exception();
            var kafka = connectionStrings["Kafka"] ?? throw new Exception();

            //Redis Dependencies
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redis));

            //Kafka Dependencies
            services.AddSingleton<IProducer<Null, string>>(provider =>
            {
                var config = new ProducerConfig
                {
                    BootstrapServers = kafka,
                };
                return new ProducerBuilder<Null, string>(config).Build();
            });

            return services;
        }
    }
}
