using Application;
using Infrastructure;
using Infrastructure.Logging.Serilog;
using Infrastructure.Persistence;
using System.Configuration;
using WebApi.Configurations;

namespace WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.AddConfigurations().RegisterSerilog();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddApplicationServices();

            builder.Services.AddInfrastructureServices(builder.Configuration);

            var app = builder.Build();

            // Database Initializer
            await app.Services.AddDatabaseInitializerAsync();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            //app.UseAuthorization();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.MapControllers();

            app.UseInfrastructure();

            app.Run();
        }
    }
}
