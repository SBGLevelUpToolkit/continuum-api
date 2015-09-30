using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;
using Microsoft.AspNet.Identity;
using Continuum.WebApi.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace Continuum.WebApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
    
            container.RegisterType<Continuum.WebApi.Controllers.AccountController, Continuum.WebApi.Controllers.AccountController>();

            if (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME")))
            {
                container.RegisterType<Data.IContinuumDataContainer, Data.ContinuumDataContainer>(new PerRequestLifetimeManager());            
            }
            else
            {
                container.RegisterType<Data.IContinuumDataContainer, Data.Mocks.MockContainer>(new PerRequestLifetimeManager());

            }

            container.RegisterType<Data.TeamRepo, Data.TeamRepo>();
            container.RegisterType<Data.DimensionRepo, Data.DimensionRepo>();
            container.RegisterType<Data.AssessmentRepo, Data.AssessmentRepo>();
            container.RegisterType<Data.GoalRepo, Data.GoalRepo>(); 

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}