using Microsoft.AspNetCore.Identity;

namespace DataAccess.Entities;

public class ApplicationUser : IdentityUser
{
    public int TokenVersion { get; set; } = 1;
}