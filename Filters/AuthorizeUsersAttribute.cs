using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticaCore2LuisEF.Filters
{
    public class AuthorizeUsersAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {

            string controller = context.RouteData.Values["controller"].ToString();
            string action = context.RouteData.Values["action"].ToString();
            ITempDataProvider provider = context.HttpContext.RequestServices.GetService(typeof(ITempDataProvider)) as ITempDataProvider;

            var TempData = provider.LoadTempData(context.HttpContext);

            TempData["controller"] = controller;
            TempData["action"] = action;

            provider.SaveTempData(context.HttpContext, TempData);

            var user = context.HttpContext.User;
            if (user.Identity.IsAuthenticated == false)
            {
                RouteValueDictionary rutalogin = new RouteValueDictionary(new
                {
                    controller = "Manage",
                    action = "Login"
                });
                RedirectToRouteResult result = new RedirectToRouteResult(rutalogin);
                context.Result = result;
            }
        }
    }
}
