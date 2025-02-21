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
            builder.Services.AddCheckUserSecrets(builder.Configuration);
            builder.Services.AddDependencies(builder.Configuration);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            await CreateKafkaTopicsAsync(app);

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

                var result = await producer.ProduceAsync(topic, message);
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