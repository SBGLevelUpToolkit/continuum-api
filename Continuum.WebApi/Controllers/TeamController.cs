using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Continuum.WebApi.Controllers
{
    public class TeamController : ApiController
    {
        public void CreateTeam(Models.Team team)
        {

            //Create the team and make set the user as the team lead. 

            //Try to get user from token.
        }

        public IEnumerable<Models.Team> Get()
        {
            return new List<Models.Team>() { new Models.Team() { Name="Team 1" }, new Models.Team() { Name = "Team 2" } };
        }
    }
}
