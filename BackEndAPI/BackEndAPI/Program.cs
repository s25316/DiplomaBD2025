
using Domain;
using Infrastructure;
using UseCase;


// Scaffold-DbContext "ConnectionString" Microsoft.EntityFrameworkCore.SqlServer -OutputDir RelationalDatabase -Project UseCase
namespace BackEndAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetSection("ConnectionStrings")["Redis"];
                //options.InstanceName = "MyAppCache_"; // Prefiks kluczy w Redis
            });
            builder.Services.DomainConfiguration(builder.Configuration);
            builder.Services.UseCaseConfiguration(builder.Configuration);
            builder.Services.InfrastructureConfiguration(builder.Configuration);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}