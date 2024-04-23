using Application.Features.Tenancy.Models;

namespace Application.Features.Tenancy
{
    public interface ITenancySubscriptionService
    {
        Task<int> CreateTenancySubscriptionAsync(CreateTenancySubscriptionRequest subscriptionRequest, CancellationToken ct);
        Task<TenancySubscriptionDto> GetTenancySubscriptionByIdAsync(int Id);
        Task<List<TenancySubscriptionDto>> GetTenancySubscriptionsAsync();
    }
}
