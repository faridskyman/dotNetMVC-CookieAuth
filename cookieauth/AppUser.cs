using Microsoft.AspNet.Identity.EntityFramework;

namespace cookieauth
{
    public class AppUser : IdentityUser
    {
        

        //public string Name { get; set; } 

        public string Country { get; set; } 

        public string WorkEmail { get; set; } 

        //public string GivenName { get; set; } 

        //public string Role { get; set; } 

        //public string UserData { get; set; } 

        //public string NameIdentifier { get; set; }

        public int Age { get; set; }

    }
}
 