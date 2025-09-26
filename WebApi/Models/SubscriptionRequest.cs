namespace WebApi.Models
{
    public class SubscriptionRequest
    {
        public string AccountId { get; set; } = null!;

        public string SubscriptionStatus { get; set; } = null!;

        public string CustomerId { get; set; } = null!;
    }
}
