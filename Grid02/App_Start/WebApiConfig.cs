using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

/**
* The School Navigator
*
* @author  Vaishnavi Sinha
* @version 1.0
* @Date   05-01-2019 
*/
namespace Grid02
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
