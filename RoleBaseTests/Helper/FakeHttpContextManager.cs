using NSubstitute;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RoleBaseTests.Helper
{
    public static partial class FakeHttpContextManager
    {
        public static HttpContextBase CreateHttpContextBase()
        {
            var context = Substitute.For<HttpContextBase>();
            var request = Substitute.For<HttpRequestBase>();
            var response = Substitute.For<HttpResponseBase>();
            var sessionState = Substitute.For<HttpSessionStateBase>();
            var serverUtility = Substitute.For<HttpServerUtilityBase>();
          
            context.Request.Returns(request);
            context.Response.Returns(response);
            context.Session.Returns(sessionState);
            context.Server.Returns(serverUtility);

            return context;
        }

        public static HttpContextBase SetIdentity(this HttpContextBase httpContextBase, string name, bool isAuthenticated = true)
        {
            var principal = Substitute.For<IPrincipal>();
            var identity = Substitute.For<IIdentity>();
            principal.Identity.Returns(identity);
            httpContextBase.User.Returns(principal);

            identity.Name.Returns(name);
            identity.IsAuthenticated.Returns(isAuthenticated);

            return httpContextBase;
        }

        public static HttpContextBase SetQueryString(this HttpContextBase httpContext, string url)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }
            if (!url.StartsWith("~/"))
            {
                //throw new ArgumentException("Sorry, we expect a virtual url starting with \"~/\".");
            }

            httpContext.Request.QueryString.Returns(GetQueryStringParameters(url));
            //httpContext.Request.AppRelativeCurrentExecutionFilePath.Returns(GetUrlFileName(url));
            //httpContext.Request.PathInfo.Returns("");

            return httpContext;
        }

        public static HttpContextBase AddSession(this HttpContextBase httpContext, string name, object value)
        {
            httpContext.Session[name].Returns(value);
            return httpContext;
        }

        private static NameValueCollection GetQueryStringParameters(string url)
        {
            if (url.Contains("?"))
            {
                NameValueCollection parameters = new NameValueCollection();
                string[] parts = url.Split("?".ToCharArray());
                string[] keys = parts[1].Split("&".ToCharArray());
                foreach (string key in keys)
                {
                    string[] part = key.Split("=".ToCharArray());
                    parameters.Add(part[0], part[1]);
                }
                return parameters;
            }
            else
            {
                return null;
            }
        }

        private static string GetUrlFileName(string url)
        {
            if (url.Contains("?"))
                return url.Substring(0, url.IndexOf("?"));
            else
                return url;
        }
    }
}
