﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;

namespace MyVoiceMVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //GoogleCloudServices require this credentials file. 
            //https://cloud.google.com/text-to-speech/docs/quickstart-client-libraries
            var path = System.Web.Hosting.HostingEnvironment.MapPath("~/Phrases-276aabe6b3ad.json");
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path, EnvironmentVariableTarget.Process);
        }
        
        protected void Session_Start(object sender, EventArgs e)
        {

        }


        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_PostAuthorizeRequest()
        {
            //if (IsWebApiRequest())
            //{
            //    HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            //}
        }


        private bool IsWebApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(WebApiConfig.UrlPrefixRelative);
        }
    }
}
