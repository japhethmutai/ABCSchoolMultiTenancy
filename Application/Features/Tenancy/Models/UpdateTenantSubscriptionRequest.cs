namespace Application.Features.Tenancy.Models
{
    public class UpdateTenantSubscriptionRequest
    {
        public string TenantId { get; set; }
        public DateTime NewExpiryDate { get; set; }
    }
}
