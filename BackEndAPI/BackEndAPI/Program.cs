// Scaffold-DbContext "ConnectionString" Microsoft.EntityFrameworkCore.SqlServer -OutputDir RelationalDatabase -Project UseCase
using Confluent.Kafka;

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
            var producer = app.Services.GetRequiredService<IProducer<Null, string>>();
            var topic = "application-start";

            try
            {
                var message = new Message<Null, string>
                {
                    Value = "Aplikacja zosta³a uruchomiona"
                };

                _ = await producer.ProduceAsync(topic, message);
            }
            catch (ProduceException<Null, string> e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        //=================================================================================================
    }
}