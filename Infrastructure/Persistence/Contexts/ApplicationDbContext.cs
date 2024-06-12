using Domain.Entities;
using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext(ITenantInfo tenantInfo, DbContextOptions<ApplicationDbContext> options, IOptions<DatabaseSettings> dbSettings)
        : BaseDbContext(tenantInfo, options, dbSettings)
    {
        public DbSet<School> Schools => Set<School>();
        public DbSet<Teacher> Teachers => Set<Teacher>();
    }
}
