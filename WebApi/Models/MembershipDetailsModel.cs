using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace WebApi.Models;

public class MembershipDetailsModel
{
    public int Id { get; set; }
    public String Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public List<string> Benefits { get; set; } = new();
}
