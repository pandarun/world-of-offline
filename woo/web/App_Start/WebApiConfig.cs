using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json.Serialization;

namespace web
{
    public static class WebApiConfig
    {
        public class TokenIdentity : IIdentity
        {
            public TokenIdentity(string name)
            {
                Name = name;

                AuthenticationType = "token";
                IsAuthenticated = true;
            }

            public string Name { get; private set; }
            public string AuthenticationType { get; private set; }
            public bool IsAuthenticated { get; private set; }
        }

        public class TokenPrincipal : IPrincipal
        {
            public bool IsInRole(string role)
            {
                throw new NotImplementedException();
            }

            public TokenPrincipal(string name)
            {
                Identity = new TokenIdentity(name);
            }
            public IIdentity Identity { get; private set; }
        }

        public class TokenFilter : System.Web.Http.AuthorizeAttribute
        {
            public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
            {
                var token = actionContext.Request.Headers.Contains("X-Auth-Token") ? actionContext.Request.Headers.GetValues("X-Auth-Token").FirstOrDefault() : null;

                if (string.IsNullOrEmpty(token))
                {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                }

                HttpContext.Current.User = new TokenPrincipal(token);
            }
        }

        public static void Register(HttpConfiguration config)
        {
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            config.Formatters.Remove(config.Formatters.XmlFormatter);
            //config.Filters.Add(new TokenFilter());
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
