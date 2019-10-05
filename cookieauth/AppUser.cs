using System.Security.Claims;

namespace cookieauth
{
    public class AppUser : ClaimsPrincipal
    {
        public AppUser(ClaimsPrincipal principal) : base(principal)
        {
        }

        public string Name { get { return this.FindFirst(ClaimTypes.Name).Value; } }

        public string Country { get { return this.FindFirst(ClaimTypes.Country).Value; } }

        public string Email { get { return this.FindFirst(ClaimTypes.Email).Value; } }

        public string GivenName { get { return this.FindFirst(ClaimTypes.GivenName).Value; } }

        public string Role { get { return this.FindFirst(ClaimTypes.Role).Value; } }

        public string UserData { get { return this.FindFirst(ClaimTypes.UserData).Value; } }

        public string NameIdentifier { get { return this.FindFirst(ClaimTypes.NameIdentifier).Value; } }

    }
}
 