using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ExtendedCrud.Models;

namespace ExtendedCrud.Data
{
    public class AddDbContext : IdentityDbContext<AppUser>
    {
        public AddDbContext(DbContextOptions<AddDbContext> options) : base(options) { }
    }
}
