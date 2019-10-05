using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace cookieauth.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            var model = new LogInModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }


        [HttpPost]
        public ActionResult Login(LogInModel model)
        {
            if (!ModelState.IsValid)
                return View();


            //a quick demo of auth, this will be linked to system actual auth
            if(model.Email == "farid@admin.com" && model.Password == "password")
            {
                // Claims are persisted to the client inside the authentication cookie
                // we also provide the authentication type. This should match the value you provided in the Startup class.
                var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, "Farid"),
                    new Claim(ClaimTypes.Email, "farid@admin.com"),
                    new Claim(ClaimTypes.Country, "Singapore"),
                    new Claim(ClaimTypes.GivenName, "Farid Ismail"),
                    new Claim(ClaimTypes.Role, "Sales Dev"),
                    new Claim(ClaimTypes.UserData, "<inst>1,2,3,4,5,6</inst>"),
                    new Claim(ClaimTypes.NameIdentifier, "1234-12345-12345-1234-1234")
                }, "ApplicationCookie");


                var ctx = Request.GetOwinContext();

                // Obtain an IAuthenticationManager instance from the current OWIN context. 
                //  This was automatically registered for you during startup.
                var authManager = ctx.Authentication;

                // Then we call IAuthenticationManager.SignIn passing the claims identity. This sets the authentication cookie on the client.
                authManager.SignIn(identity);

                // redirect the user agent to the resource they attempted to access.
                return Redirect(GetRedirectUrl(model.ReturnUrl));
            }

            //if user auth fails
            ModelState.AddModelError("", "Invalid email or password");
            return View();
            
        }

        /// <summary>
        /// We also check to ensure the return URL is local to the application to prevent Open Redirection attacks.
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        private string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("index", "home");
            }

            return returnUrl;
        }

        /// <summary>
        /// Obtain the IAuthenticationManager instance from the OWIN context, 
        /// calling SignOut passing the authentication type (so the manager knows exactly what cookie to remove).
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOut()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("index", "home");
        }
    }
}