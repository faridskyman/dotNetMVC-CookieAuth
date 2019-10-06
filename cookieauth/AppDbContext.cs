using Microsoft.AspNet.Identity.EntityFramework;

namespace cookieauth
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext() : base("DefaultConnection") {}
    }
}