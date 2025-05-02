// Scaffold-DbContext "ConnectionString" Microsoft.EntityFrameworkCore.SqlServer -OutputDir RelationalDatabase -Project UseCase
using UseCase.Kafka;
using UseCase.MongoDb.UserLogs.Models.ServiceEvents;

namespace BackEndAPI
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDependencies(builder.Configuration);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();
            await CreateKafkaTopicsAsync(app);

            // Configure the requesting from any host
            app.UseCors(x => x.AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

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