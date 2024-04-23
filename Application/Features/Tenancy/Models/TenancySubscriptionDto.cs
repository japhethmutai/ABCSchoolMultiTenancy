namespace Application.Features.Tenancy.Models
{
    public class TenancySubscriptionDto
    {
        public int Id { get; set; }
        public DateTime SubscriptionDate { get; set; }
        public int SubscriptionAmount { get; set; }
        public int PeriodInMonths { get; set; }
        public string TenantId { get; set; }
    }
}
