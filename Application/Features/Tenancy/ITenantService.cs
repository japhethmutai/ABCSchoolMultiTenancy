using Application.Features.Tenancy.Models;

namespace Application.Features.Tenancy
{
    public interface ITenantService
    {
        // Creation of the tenant
        Task<string> CreateTenantAsync(CreateTenantRequest createTenant, CancellationToken cancellationToken);
        Task<string> ActivateAsync(string Id);
        Task<string> DeactivateAsync(string Id);
        Task<string> UpgradeSubscriptionAsync(string Id, DateTime newExpiryDate);
        Task<List<TenantDto>> GetTenantsAsync(bool isActive = true);
        Task<TenantDto> GetTenantByIdAsync(string Id);
    }
}
