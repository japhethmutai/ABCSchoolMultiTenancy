using Domain.Entities;
using Finbuckle.MultiTenant.Stores;
using Infrastructure.Persistence.DbConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tenancy
{
    public class TenantDbContext(DbContextOptions<TenantDbContext> options) : EFCoreStoreDbContext<ABCSchoolTenantInfo>(options)
    {
        public DbSet<TenancySubscription> TenancySubscriptions => Set<TenancySubscription>();
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ABCSchoolTenantInfo>()
                .ToTable("Tenants", SchemaNames.Multitenancy);

            builder.Entity<TenancySubscription>()
                .ToTable("TenancySubscriptions", SchemaNames.Multitenancy);
        }
    }
}
