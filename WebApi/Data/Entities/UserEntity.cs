using Microsoft.AspNetCore.Identity;

namespace WebApi.Data.Entities;

public class UserEntity : IdentityUser
{
    public int MembershipId { get; set; }

    public string SubscriptionStatus { get; set; } = "None";

    public string CustomerId { get; set; } = null!;
}
