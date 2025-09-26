namespace WebApi.Models;

public class ProfileModel
{
    public string UserId { get; set; } = null!;

    public int MembershipId { get; set; }

    public string SubscriptionStatus { get; set; } = null!;

    public string? CustomerId { get; set; }

    public string Email { get; set; } = null!;
}
