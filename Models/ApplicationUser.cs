using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace PPE.Models;

[Keyless]
public class ApplicationUser : IdentityUser
{
    public int? ProjectId { get; set; }
    public Project? Project { get; set; }
}