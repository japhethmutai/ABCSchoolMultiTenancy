using Application.Exceptions;
using Application.Features.Tenancy;
using Application.Features.Tenancy.Models;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Tenancy
{
    public class TenancySubscriptionService(IMultiTenantStore<ABCSchoolTenantInfo> tenantStore, TenantDbContext tenantDbContext) : ITenancySubscriptionService
    {
        private readonly IMultiTenantStore<ABCSchoolTenantInfo> _tenantStore = tenantStore;
        private readonly TenantDbContext _tenantDbContext = tenantDbContext;

        public async Task<int> CreateTenancySubscriptionAsync(CreateTenancySubscriptionRequest subscriptionRequest, CancellationToken ct)
        {
            var tenantInDb = await _tenantStore.TryGetAsync(subscriptionRequest.TenantId)
                ?? throw new NotFoundExceptions("Tenant does not exist.");

            var newTenantSubscriptions = new TenancySubscription()
            {
                TenantId = tenantInDb.Id,
                SubscriptionDate = subscriptionRequest.SubscriptionDate,
                SubscriptionAmount = subscriptionRequest.SubscriptionAmount,
                PeriodInMonths = subscriptionRequest.PeriodInMonths,
            };

            await _tenantDbContext.TenancySubscriptions.AddAsync(newTenantSubscriptions, ct);
            await _tenantDbContext.SaveChangesAsync(ct);

            tenantInDb.ValidUpTo = tenantInDb.ValidUpTo.AddMonths(newTenantSubscriptions.PeriodInMonths);
            tenantInDb.IsActive = true;
            await _tenantStore.TryUpdateAsync(tenantInDb);

            return newTenantSubscriptions.Id;
        }

        public Task<TenancySubscriptionDto> GetTenancySubscriptionByIdAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<List<TenancySubscriptionDto>> GetTenancySubscriptionsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
