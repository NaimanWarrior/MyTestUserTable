using Microsoft.AspNetCore.Identity;

namespace ExtendedCrud.Models
{
    public class AppUser : IdentityUser
    {
        public bool IsBlocked { get; set; } = false;
    }
}
