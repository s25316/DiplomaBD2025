// Scaffold-DbContext "ConnectionString" Microsoft.EntityFrameworkCore.SqlServer -OutputDir RelationalDatabase -Project UseCase
using BackEndAPI.Middlewares;
using Microsoft.AspNetCore.Authentication;
using UseCase.MongoDb.UserLogs.Models.ServiceEvents;
using UseCase.Shared.Interfaces;

namespace BackEndAPI
{
    public class Program
    {
        // Properties
        private static string? _baseAdministratorId = "23e478b0-13cf-4038-8b89-3f307cd992e7";
        public static Guid BaseAdministrator => Guid.TryParse(_baseAdministratorId, out var id)
            ? id
            : Guid.Empty;


        // Methods
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddTransient<IClaimsTransformation, ClaimsTransformation>();
            builder.Services.AddDependencies(builder.Configuration);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddHttpContextAccessor(); // For get in IClaimsTransformation source data
            var app = builder.Build();


            await CreateKafkaTopicsAsync(app);
            app.UseGlobalErrorHandlingMiddleware();
            app.UseUserAuthorizationMiddleware();

            // Configure the requesting from any host
            app.UseCors(x => x.AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }

        //=================================================================================================
        //Private Methods
        private static async Task CreateKafkaTopicsAsync(WebApplication app)
        {
            var producer = app.Services.GetRequiredService<IKafkaService>();
            var applicationRun = ApplicationRunMongoDb.Prepare();

            foreach (var topic in UseCase.Configuration.KafkaTopics)
            {
                await producer.SendUserLogAsync(
                    applicationRun,
                    CancellationToken.None);
            }
        }
        //=================================================================================================
    }
}