using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebAPI4AngularCosmosDB.Models;

namespace WebAPI4AngularCosmosDB
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Make sure to change this to the right name for your repository to connect.
            //Make sure to have the emulator downloaded and set up before you run this.
            //To eliminate possible bug errors with the controller, try running the API and if the repository
            //is created in the emulator, you know it is working
            CosmosDBRepository<Hero>.Initialize();
        }
    }
}
