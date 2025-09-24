using Microsoft.AspNetCore.Identity;

namespace WebApi.Data.Entities;

public class UserEntity : IdentityUser
{
    public int MembershipId { get; set; }
}
