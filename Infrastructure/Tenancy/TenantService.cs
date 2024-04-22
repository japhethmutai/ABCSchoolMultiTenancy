using Application.Features.Tenancy;
using Application.Features.Tenancy.Models;
using Finbuckle.MultiTenant;
using Infrastructure.Persistence.DbInitializers;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Tenancy
{
    public class TenantService(IMultiTenantStore<ABCSchoolTenantInfo> multiTenantStore, 
        ApplicationDbInitializer applicationDbInitializer,
        IServiceProvider serviceProvider) : ITenantService
    {
        private readonly IMultiTenantStore<ABCSchoolTenantInfo> _tenantStore = multiTenantStore;
        private readonly ApplicationDbInitializer _applicationDbInitializer = applicationDbInitializer;
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task<string> ActivateAsync(string Id)
        {
            var tenantInDb = await _tenantStore.TryGetAsync(Id);
            tenantInDb.IsActive = true;

            await _tenantStore.TryUpdateAsync(tenantInDb);
            return tenantInDb.Id;
        }

        public async Task<string> CreateTenantAsync(CreateTenantRequest createTenant, CancellationToken cancellationToken)
        {
            // Create tenant in Db
            var newTenant = new ABCSchoolTenantInfo
            {
                Id = createTenant.Identifier,
                Identifier = createTenant.Identifier,
                Name = createTenant.Name,
                ConnectionString = createTenant.ConnectionString,
                AdminEmail = createTenant.AdminEmail,
                ValidUpTo = createTenant.ValidUpTo,
                IsActive = createTenant.IsActive,
            };

            await _tenantStore.TryAddAsync(newTenant);

            // Initialize tenant with Users, User Roles, Roles and Role Permissions
            try
            {
                using var scope = _serviceProvider.CreateScope();
                _serviceProvider.GetRequiredService<IMultiTenantContextAccessor>()
                    .MultiTenantContext = new MultiTenantContext<ABCSchoolTenantInfo>()
                    {
                        TenantInfo = newTenant
                    };
                await scope.ServiceProvider.GetRequiredService<ApplicationDbInitializer>()
                    .InitializeDatabaseAsync(cancellationToken);
            }
            catch (Exception)
            {
                await _tenantStore.TryRemoveAsync(createTenant.Identifier);
                throw;
            }


            return newTenant.Id;
        }

        public async Task<string> DeactivateAsync(string Id)
        {
            var tenantInDb = await _tenantStore.TryGetAsync(Id);
            tenantInDb.IsActive = false;

            await _tenantStore.TryUpdateAsync(tenantInDb);
            return tenantInDb.Id;
        }

        public async Task<TenantDto> GetTenantByIdAsync(string Id)
        {
            var tenantInDb = await _tenantStore.TryGetAsync(Id);

            #region Manual Mapping - Opt 1
            //return new TenantDto()
            //{
            //    Id = tenantInDb.Id,
            //    Name = tenantInDb.Name,
            //    AdminEmail = tenantInDb.AdminEmail,
            //    ConnectionString = tenantInDb.ConnectionString,
            //    ValidUpTo = tenantInDb.ValidUpTo,
            //    IsActive = tenantInDb.IsActive,
            //};
            #endregion

            // Using a mapping library - Opt 2 (MapSter)
            return tenantInDb.Adapt<TenantDto>();
        }

        public async Task<List<TenantDto>> GetTenantsAsync(bool isActive = true)
        {
            var tenantsInDb = await _tenantStore.GetAllAsync();
            return tenantsInDb.Adapt<List<TenantDto>>();
        }

        public async Task<string> UpgradeSubscriptionAsync(string Id, DateTime newExpiryDate)
        {
            var tenantInDb = await _tenantStore.TryGetAsync(Id);
            tenantInDb.ValidUpTo = newExpiryDate;
            tenantInDb.IsActive = true;
            await _tenantStore.TryUpdateAsync(tenantInDb);
            return tenantInDb.Id;
        }
    }
}
