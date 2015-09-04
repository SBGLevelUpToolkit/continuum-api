using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Continuum.WebApi.Controllers
{
    public class ControllerBase : System.Web.Http.ApiController
    {
        public static System.Security.Principal.IPrincipal CurrentUser; 

        private readonly Data.ITeamRepo _teamRepo; 
        public ControllerBase(Data.ITeamRepo teamRepo)
        {
            _teamRepo = teamRepo; 
        }

        protected Data.Team GetTeamForUser()
        {
            var team = _teamRepo.GetTeamForUser(User.Identity.Name).FirstOrDefault();
            if (team == null)
            {
                throw ExceptionBuilder.CreateInternalServerError(String.Format("{0} is not a member of a team.", User.Identity.Name), "No Team.");
            }
            return team;
        }

    }
}