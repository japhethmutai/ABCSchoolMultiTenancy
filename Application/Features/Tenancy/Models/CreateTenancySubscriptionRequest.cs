namespace Application.Features.Tenancy.Models
{
    public class CreateTenancySubscriptionRequest
    {
        public DateTime SubscriptionDate { get; set; }
        public int SubscriptionAmount { get; set; }
        public int PeriodInMonths { get; set; }
        public string TenantId { get; set; }
    }
}
