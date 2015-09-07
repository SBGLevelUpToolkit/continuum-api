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
                container.RegisterInstance<Data.IContinuumDataContainer>(new Data.ContinuumDataContainer());
            }
            else
            {
                container.RegisterInstance<Data.IContinuumDataContainer>(new Data.Mocks.MockContainer());

            }

            container.RegisterType<Data.TeamRepo, Data.TeamRepo>();
            container.RegisterType<Data.DimensionRepo, Data.DimensionRepo>();
            container.RegisterType<Data.AssessmentRepo, Data.AssessmentRepo>();
            container.RegisterType<Data.GoalRepo, Data.GoalRepo>(); 

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}