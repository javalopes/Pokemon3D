using System.Net.Http.Headers;
using System.Web.Http;
using Microsoft.Practices.Unity;
using Pokemon3D.Master.Server.Services;

namespace Pokemon3D.Master.Server
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer();
            container.RegisterType<IRegistrationService, RegistrationService>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);
            
            config.MapHttpAttributeRoutes();

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
