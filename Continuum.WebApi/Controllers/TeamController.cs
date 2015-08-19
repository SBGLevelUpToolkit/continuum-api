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
        private readonly Data.IRepository<Data.Team> _teamRepo;

        public TeamController(Data.IRepository<Data.Team> teamRepo)
        {
            _teamRepo = teamRepo;
        }

        public void Put(Models.Team team)
        {
            if (_teamRepo.All().Any(i => i.Name == team.Name.Trim()))
            {
                var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(string.Format("A Team called {0} already exists.", team.Name)),
                    ReasonPhrase = "Duplicate Team"
                };
                
                throw new HttpResponseException(resp);
            }
        }

        public IEnumerable<Models.Team> Get()
        {
            return new List<Models.Team>() { 
                new Models.Team() { Name="Team 1", TeamLeadName = "Bob" }, 
                new Models.Team() { Name = "Team 2", TeamLeadName = "Fred" } };
        }
    }
}
