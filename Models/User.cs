using Microsoft.AspNetCore.Identity;

namespace apiLeran.Models;

public class User : IdentityUser
{
    public string Name { get; set; }
}