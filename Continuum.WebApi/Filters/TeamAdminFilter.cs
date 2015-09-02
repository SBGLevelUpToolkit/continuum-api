using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace Continuum.WebApi.Filters
{
    public class TeamAdminFilter : AuthorizeAttribute
    {
        protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            bool result = false;

            if (base.IsAuthorized(actionContext))
            {
                IPrincipal client = Thread.CurrentPrincipal;


                Data.ITeamRepo teamRepo = (Data.ITeamRepo)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(Data.ITeamRepo));
                var team = teamRepo.GetTeamForUser(client.Identity.Name).FirstOrDefault();
                if (team != null)
                {
                    result = team.TeamMembers.Any(i => i.UserId == client.Identity.Name && i.IsAdmin);
                }
            }

            return result;
        }
    }
}