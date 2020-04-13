using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoleBase.ActionFilters
{
    public class AllowCrossSiteAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            //filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type");
            //filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Methods", "POST");
            ////filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Credentials", "true");

            //base.OnActionExecuting(filterContext);

            HttpRequest request = HttpContext.Current.Request;
            HttpResponse response = HttpContext.Current.Response;

            // check for preflight request
            if (request.Headers.AllKeys.Contains("Origin") && request.HttpMethod == "OPTIONS")
            {
                response.AppendHeader("Access-Control-Allow-Origin", "*");
                response.AppendHeader("Access-Control-Allow-Credentials", "true");
                response.AppendHeader("Access-Control-Allow-Methods", "GET, PUT, POST, DELETE");
                response.AppendHeader("Access-Control-Allow-Headers", "Origin, X-Requested-With, X-RequestDigest, Cache-Control, Content-Type, Accept, Access-Control-Allow-Origin, Session, odata-version");
                response.End();
            }
            else
            {
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetNoStore();

                response.AppendHeader("Access-Control-Allow-Origin", "*");
                response.AppendHeader("Access-Control-Allow-Credentials", "true");
                if (request.HttpMethod == "POST")
                {
                    response.AppendHeader("Access-Control-Allow-Methods", "GET, PUT, POST, DELETE");
                    response.AppendHeader("Access-Control-Allow-Headers", "Origin, X-Requested-With, X-RequestDigest, Cache-Control, Content-Type, Accept, Access-Control-Allow-Origin, Session, odata-version");
                }

                base.OnActionExecuting(filterContext);
            }
        }
    }
}