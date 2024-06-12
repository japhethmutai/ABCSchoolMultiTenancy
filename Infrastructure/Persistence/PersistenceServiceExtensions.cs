using Infrastructure.Common;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.DbInitializers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;

namespace Infrastructure.Persistence
{
    public static class PersistenceServiceExtensions
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(PersistenceServiceExtensions));
        public static IServiceCollection AddPersistenceService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<DatabaseSettings>()
                .BindConfiguration(nameof(DatabaseSettings))
                .PostConfigure(databaseSettings =>
                {
                    _logger.Information("Current DB Provider: {dbProvider}", databaseSettings.DBProvider);
                })
                .ValidateDataAnnotations()
                .ValidateOnStart();

            return services
                .AddDbContext<ApplicationDbContext>((p, m) =>
                {
                    var databaseSettings = p.GetRequiredService<IOptions<DatabaseSettings>>().Value;
                    m.UseDatabase(databaseSettings.DBProvider, databaseSettings.ConnectionString);
                })
                //.AddDbContext<ApplicationDbContext>(options => options
                //    .UseNpgsql(configuration.GetConnectionString("DefaultConnection")))
                //.UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
                .AddTransient<ITenantDbInitializer, TenantDbInitializer>()
                .AddTransient<ApplicationDbInitializer>();
        }

        public static async Task AddDatabaseInitializerAsync(this IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
        {
            using var scope = serviceProvider.CreateScope();

            await scope.ServiceProvider.GetRequiredService<ITenantDbInitializer>()
                .InitializeDatabaseAsync(cancellationToken);
        }

        internal static DbContextOptionsBuilder UseDatabase(this DbContextOptionsBuilder builder, string dbProvider, string connectionString)
        {
            return dbProvider.ToLowerInvariant() switch
            {
                DbProviderKeys.Npgsql => builder.UseNpgsql(connectionString, e =>
                                     e.MigrationsAssembly("Migrators.PostgreSQL")),
                DbProviderKeys.SqlServer => builder.UseSqlServer(connectionString, e =>
                                     e.MigrationsAssembly("Migrators.MSSQL")),
                _ => throw new InvalidOperationException($"DB Provider {dbProvider} is not supported."),
            };
        }
    }
}
