// Ignore Spelling: Jwt, Redis, Sql, Mongo
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using UseCase.Roles.CompanyUser.Repositories.Branches;
using UseCase.Roles.CompanyUser.Repositories.Companies;
using UseCase.Roles.CompanyUser.Repositories.ContractConditions;
using UseCase.Roles.CompanyUser.Repositories.Offers;
using UseCase.Roles.CompanyUser.Repositories.OfferTemplates;
using UseCase.Roles.Users.Repositories;
using UseCase.Shared.Dictionaries.Repositories;
using UseCase.Shared.Exceptions;
using UseCase.Shared.Repositories.Addresses;
using UseCase.Shared.Repositories.RecruitmentMessages;
using UseCase.Shared.Repositories.Recruitments;
using UseCase.Shared.Services.Authentication.Generators;
using UseCase.Shared.Services.Authentication.Inspectors;


namespace UseCase
{
    public static class Configuration
    {
        // Properties
        public static string RelationalDatabaseConnectionString { get; private set; } = null!;
        public static string RedisConnectionString { get; private set; } = null!;
        public static string KafkaConnectionString { get; private set; } = null!;
        public static string KafkaTopicUserLogs { get; private set; } = "user-logs";
        // If You create new Kafka TOPIC add here for automatically creating by side Kafka
        public static IEnumerable<string> KafkaTopics { get; private set; } = new List<string>
        {
            KafkaTopicUserLogs,
        };
        public static string MongoDbConnectionString { get; private set; } = null!;
        public static string MongoDbDatabase { get; private set; } = null!;
        public static string MongoCollectionUserLogs { get; private set; } = "user-logs";


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
            // MongoDb
            MongoDbConnectionString = configuration.GetSection("ConnectionStrings")["MongoDb"] ??
                throw new UseCaseLayerException(Messages.UserSecrets_NotFound_MongoDb);
            MongoDbDatabase = configuration.GetSection("ConnectionStrings")["MongoDbDatabase"] ??
                throw new UseCaseLayerException(Messages.UserSecrets_NotFound_MongoDbDatabase);

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
            // Main Configuration
            services.AddMediatR(config => config.RegisterServicesFromAssembly(
                Assembly.GetExecutingAssembly()
                ));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());


            // Company User
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<IBranchRepository, BranchRepository>();
            services.AddTransient<IOfferTemplateRepository, OfferTemplateRepository>();
            services.AddTransient<IContractConditionRepository, ContractConditionRepository>();
            services.AddTransient<IOfferRepository, OfferRepository>();

            // User
            services.AddTransient<IPersonRepository, PersonRepository>();

            // Guest

            // Shared
            services.AddTransient<IDictionariesRepository, DictionariesRepository>();
            services.AddTransient<IAddressRepository, AddressRepository>();
            services.AddTransient<IRecruitmentMessagesRepository, RecruitmentMessagesRepository>();

            services.AddTransient<IAuthenticationGeneratorService, AuthenticationGeneratorService>();
            services.AddTransient<IAuthenticationInspectorService, AuthenticationInspectorService>();
            services.AddTransient<IRecruitmentRepository, RecruitmentRepository>();
            return services;
        }
    }
}
