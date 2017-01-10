using Microsoft.Practices.Unity;
using System.Web.Http;
using Pokemon3D.Master.Server.Services;
using Unity.WebApi;

namespace Pokemon3D.Master.Server
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            container.RegisterType<IRegistrationService, RegistrationService>(new ContainerControlledLifetimeManager());
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}