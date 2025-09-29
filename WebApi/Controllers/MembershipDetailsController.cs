using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

[ApiController]
[Route("api/[controller]")]
public class MembershipDetailsController : ControllerBase
{
    [HttpGet("{id}")]
    public IActionResult GetMembershipDetails(int id)
    {
        MembershipDetailsModel? details = id switch
        {
            1 => new MembershipDetailsModel
            {
                Id = 1,
                Name = "Basic",
                Price = 19,
                Benefits = new List<string>
                {
                    "Access to gym equipment",
                    "Group classes"
                }
            },
            2 => new MembershipDetailsModel
            {
                Id = 2,
                Name = "Standard",
                Price = 29,
                Benefits = new List<string>
                {
                    "Everything in Basic",
                    "1 PT session per month"
                }
            },
            3 => new MembershipDetailsModel
            {
                Id = 3,
                Name = "Premium",
                Price = 39,
                Benefits = new List<string>
                {
                    "All Standard perks",
                    "24/7 access",
                    "Free drinks"
                }
            },
            _ => null
        };

        if (details == null)
            return NotFound(new { message = "Membership not found" });

        return Ok(details);
    }
}
