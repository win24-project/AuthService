namespace WebApi.Models;

public class ProfileModel
{
    public string UserId { get; set; } = null!;

    public int MembershipId { get; set; }

    public string Email { get; set; } = null!;
}
