﻿using System.Security.Claims;
using System.Web.Mvc;


namespace cookieauth
{
    public abstract class AppViewPage<TModel> : WebViewPage<TModel>
    {
        protected AppUser CurrentUser
        {
            get { return new AppUser(this.User as ClaimsPrincipal); }
        }
    }

    public abstract class AppViewPage : AppViewPage<dynamic>
    {
    }
}