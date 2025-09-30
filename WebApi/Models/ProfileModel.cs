namespace WebApi.Models;

public class ProfileModel
{
    public string UserId { get; set; } = null!;

    public string SubscriptionStatus { get; set; } = null!;

    public string MemebershipPlan { get; set; } = null!;

    public string? CustomerId { get; set; }

    public string Email { get; set; } = null!;
}
