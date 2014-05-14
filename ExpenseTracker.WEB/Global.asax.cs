using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ExpenseTracker.WEB
{
    using log4net;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure(); 
            var log = LogManager.GetLogger(this.GetType());
            log.Info("Global ASAX Running");

            AreaRegistration.RegisterAllAreas();
            this.RegisterRoutes(RouteTable.Routes);
            this.RegisterGlobalFilters(GlobalFilters.Filters);
        }

        private void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        private void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new MvcExceptionFilter());
        }
    }
}
