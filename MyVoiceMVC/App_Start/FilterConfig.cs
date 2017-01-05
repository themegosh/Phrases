using System;
using System.Web;
using System.Web.Mvc;

namespace MyVoiceMVC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomRequireHttpsFilter());
        }
    }

    public class CustomRequireHttpsFilter : RequireHttpsAttribute
    {
        protected override void HandleNonHttpsRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.Request.Url.Host.Contains("localhost"))
            {
                base.HandleNonHttpsRequest(filterContext);
            }
        }
    }
}
