using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;
using Microsoft.AspNet.Identity;
using Continuum.WebApi.Models;
using Microsoft.AspNet.Identity.EntityFramework;

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


           // var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));

            container.RegisterType<Continuum.WebApi.Controllers.AccountController, Continuum.WebApi.Controllers.AccountController>(); 


            container.RegisterType<Data.IRepository<Data.Team>, Data.TeamRepository>();
            container.RegisterType<Data.DimensionRepo, Data.DimensionRepo>();
            container.RegisterType<Data.IAssessmentRepo, Data.AssessmentRepo>();
            container.RegisterType<Data.ITeamRepo, Data.TeamRepository>();
            container.RegisterType< Data.ILookupRepo, Data.LookupRepo>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}