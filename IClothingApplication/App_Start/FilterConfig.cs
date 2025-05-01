using System;
using System.Web;
using System.Web.Mvc;

namespace IClothingApplication
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }

    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // Check if the user is logged in and is an admin
            var userType = httpContext.Session["UserType"];
            return userType != null && userType.ToString() == "admin";
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // Redirect to the homepage if unauthorized  
            filterContext.Result = new RedirectToRouteResult(
               new System.Web.Routing.RouteValueDictionary
               {
                   { "controller", "Home" },
                   { "action", "Index" }
               });
        }
    }
    //public class EnsureModelAttribute : ActionFilterAttribute
    //{
    //    public override void OnActionExecuted(ActionExecutedContext filterContext)
    //    {
    //        var result = filterContext.Result as ViewResult;
    //        if (result != null && result.Model == null)
    //        {
    //            // Check if the view expects a specific model type
    //            var modelType = result.ViewData.ModelMetadata.ModelType;
    //            if (typeof(IClothingApplication.Models.UserPassword).IsAssignableFrom(modelType))
    //            {
    //                // Initialize the model to a new instance
    //                result.ViewData.Model = Activator.CreateInstance(modelType);
    //            }
    //        }

    //        base.OnActionExecuted(filterContext);
    //    }
    //}
}
