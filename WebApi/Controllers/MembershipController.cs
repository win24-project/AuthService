using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembershipController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<MembershipModel>> GetAll()
    {
        var memberships = new List<MembershipModel>
            {
                new MembershipModel { Id = 1, Name = "Basic", Price = 19, Description = "Access to gym equipment and group classes." },
                new MembershipModel { Id = 2, Name = "Standard", Price = 29, Description = "Includes Basic + 1 PT session per month." },
                new MembershipModel { Id = 3, Name = "Premium", Price = 39, Description = "All Standard perks + 24/7 access and free drinks." }
            };

        return Ok(memberships);
    }
}
