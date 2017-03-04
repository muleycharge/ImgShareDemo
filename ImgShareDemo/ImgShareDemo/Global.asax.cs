namespace ImgShareDemo
{
    using ImgShareDemo.BLL.Static;
    using System;
    using System.Configuration;
    using System.Security.Claims;
    using System.Web.Helpers;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;

            string[] missingConfiguration;
            if(!AppConfig.Validate(out missingConfiguration))
            {
                throw new ConfigurationErrorsException($"The following configuration values are missing from application config file {String.Join(",", missingConfiguration)}.");
            }
        }
    }
}
