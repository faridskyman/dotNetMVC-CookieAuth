using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace cookieauth
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/auth/login"),
                LogoutPath = new PathString("/home/index"),
                CookieHttpOnly = true,
                CookieSecure = CookieSecureOption.Never, 
                ExpireTimeSpan = TimeSpan.FromMinutes(1),
                SlidingExpiration = true,
                CookieName = "myAuthCookie"
            });
        }
    }
}