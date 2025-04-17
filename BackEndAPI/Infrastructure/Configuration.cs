// Ignore Spelling: Kafka, redis
using Confluent.Kafka;
using Infrastructure.Exceptions;
using Infrastructure.Kafka;
using Infrastructure.Redis;
using Infrastructure.RelationalDatabase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using StackExchange.Redis;
using UseCase.Kafka;
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
            // Redis Dependency
            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(
                    UseCase.Configuration.RedisConnectionString));
            // Kafka Dependency
            services.AddSingleton<IProducer<Null, string>>(provider =>
            {
                var config = new ProducerConfig
                {
                    BootstrapServers = UseCase.Configuration.KafkaConnectionString,
                };
                return new ProducerBuilder<Null, string>(config).Build();
            });
            // MongoDb Dependency
            services.AddSingleton<IMongoClient>(
                new MongoClient(
                    UseCase.Configuration.MongoDbConnectionString));
            services.AddTransient<IMongoDatabase>(provider =>
                provider.GetService<IMongoClient>()?.GetDatabase(
                    UseCase.Configuration.MongoDbDatabase)
                ?? throw new InfrastructureLayerException("Not configured IMongoClient"));


            // Services and Repository Dependencies
            services.AddTransient<DiplomaBdContext, MsSqlDiplomaBdContext>();
            services.AddTransient<IRedisService, RedisService>();
            services.AddTransient<IKafkaService, KafkaService>();
            return services;
        }
    }
}
