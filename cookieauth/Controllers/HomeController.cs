using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace cookieauth.Controllers
{
    public class HomeController : AppController
    {
        // GET: Home
        public ActionResult Index()
        {
            // To access other claims else we can cast the current user identity as a ClaimIdentity.
            //var claimsIdentity = User.Identity as ClaimsIdentity;
            //ViewBag.Country = claimsIdentity.FindFirst(ClaimTypes.Country).Value;
            //ViewBag.Country = CurrentUser.Country;
            //ViewBag.GivenName = CurrentUser.GivenName;
            //ViewBag.Role = CurrentUser.Role;

            return View();
        }
    }
}