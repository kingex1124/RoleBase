using LoginServerBO.Service;
using LoginServerBO.Service.Interface;
using RoleBase.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RoleBase
{
    public class RouteConfig
    {

        public static UnityContainer Container;
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            Container = new UnityContainer();
            Container.Register<IRegistService, RegistService>();
            Container.Register<ILoginService, LoginService>();
            Container.Register<ISecurityService, SecurityService>();
            Container.Register<IFunctionService, FunctionService>();

        }
    }
}
