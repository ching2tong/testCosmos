using System.Web;
using System.Web.Mvc;

namespace WebAPI4AngularCosmosDB
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
