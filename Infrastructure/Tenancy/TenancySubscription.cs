namespace Infrastructure.Tenancy
{
    public class TenancySubscription
    {
        public int Id { get; set; }
        public DateTime SubscriptionDate { get; set; }
        public int SubscriptionAmount { get; set; }
        public int PeriodInMonths { get; set; }
        public string TenantId { get; set; }

        // Navigation
        public ABCSchoolTenantInfo ABCSchoolTenantInfo { get; set; }
    }
}
