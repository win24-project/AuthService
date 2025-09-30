using Microsoft.AspNetCore.Identity;

namespace WebApi.Data.Entities;

public class UserEntity : IdentityUser
{
    public int MembershipId { get; set; }

    public string MemberShipPlan { get; set; } = string.Empty;

    public string SubscriptionStatus { get; set; } = string.Empty;

    public string CustomerId { get; set; } = string.Empty;
}
