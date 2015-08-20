using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;

namespace Continuum.WebApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();


            container.RegisterType<Data.IRepository<Data.Team>, Data.TeamRepository>();
            container.RegisterType<Data.DimensionRepo, Data.DimensionRepo>();
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}