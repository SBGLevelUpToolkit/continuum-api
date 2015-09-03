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
            
            container.RegisterType<Data.IRepository<Data.Team>, Data.TeamRepository>();
            container.RegisterType<Data.DimensionRepo, Data.DimensionRepo>();
            container.RegisterType<Data.IAssessmentRepo, Data.AssessmentRepo>();
            container.RegisterType<Data.ITeamRepo, Data.TeamRepository>();
            container.RegisterType< Data.ILookupRepo, Data.LookupRepo>();
            container.RegisterType<Data.GoalRepository, Data.GoalRepository>(); 

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}