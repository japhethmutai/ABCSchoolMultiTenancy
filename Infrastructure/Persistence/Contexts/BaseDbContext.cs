using Finbuckle.MultiTenant;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.Persistence.Contexts
{
    public abstract class BaseDbContext
        : MultiTenantIdentityDbContext<ApplicationUser, ApplicationRole, string,
            IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>,
            IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        private readonly DatabaseSettings _dbSettings;

        protected BaseDbContext(ITenantInfo tenantInfo, DbContextOptions options, IOptions<DatabaseSettings> dbSettings) : base(tenantInfo, options)
        {
            _dbSettings = dbSettings.Value;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // TODO: We want this only for development probably... maybe better make it configurable in logger.json config?
            optionsBuilder.EnableSensitiveDataLogging();

            // If you want to see the sql queries that efcore executes:

            // Uncomment the next line to see them in the output window of visual studio
            // optionsBuilder.LogTo(m => System.Diagnostics.Debug.WriteLine(m), Microsoft.Extensions.Logging.LogLevel.Information);

            // Or uncomment the next line if you want to see them in the console
             optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);

            if (!string.IsNullOrEmpty(TenantInfo?.ConnectionString))
            {
                //optionsBuilder.UseSqlServer(TenantInfo.ConnectionString, options =>
                //{
                //    options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                //});
                //optionsBuilder.UseNpgsql(TenantInfo.ConnectionString, options =>
                //{
                //    options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                //})
                optionsBuilder.UseDatabase(_dbSettings.DBProvider, TenantInfo.ConnectionString);
            }
        }
    }
}
