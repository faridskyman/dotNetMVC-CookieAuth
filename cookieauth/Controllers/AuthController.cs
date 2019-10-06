using System;
using System.Collections.Generic;
using System.Linq;
//using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using cookieauth.Models;

namespace cookieauth.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> userManager;

        public AuthController() : this (Startup.UserManagerFactory.Invoke()) { }

        public AuthController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

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
        public async Task<ActionResult> Login(LogInModel model)
        {
            if (!ModelState.IsValid)
                return View();

            #region old code
            /*
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
                     
                */
            #endregion

            // First we attempt to find a user with the provided credentials using
            var user = await userManager.FindAsync(model.Email, model.Password);

            // If the user exists we create a claims identity for the user that can 
            // be passed to 'AuthenticationManager'. This will include any custom claims that you've stored.
            if (user !=null)
            {
                // Finally we sign in the user using the cookie authentication middleware SignIn(identity).
                await SignIn(user);
                return Redirect(GetRedirectUrl(model.ReturnUrl));
            }

            //if user auth fails
            ModelState.AddModelError("", "Invalid email or password");
            return View();
        }



        private async Task SignIn(AppUser user)
        {
            var identity = await userManager.CreateIdentityAsync(
                user, DefaultAuthenticationTypes.ApplicationCookie);
            GetAuthenticationManager().SignIn(identity);
        }

        private IAuthenticationManager GetAuthenticationManager()
        {
            var ctx = Request.GetOwinContext();
            return ctx.Authentication;
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


        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = new AppUser
            {
                UserName = model.Email,
                Country = model.Country,
                Age = model.Age
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await SignIn(user);
                return RedirectToAction("index", "home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }

            return View();
        }

        /// <summary>
        /// Obtain the IAuthenticationManager instance from the OWIN context, 
        /// calling SignOut passing the authentication type (so the manager knows exactly what cookie to remove).
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOut()
        {

            #region old code
            //var ctx = Request.GetOwinContext();
            //var authManager = ctx.Authentication;
            //authManager.SignOut("ApplicationCookie");
            #endregion

            GetAuthenticationManager().SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("index", "home");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing && userManager != null)
            {
                userManager.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}