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

            Data.Team newTeam = new Data.Team()
            {
                Name = team.Name
            };

            _teamRepo.Create(newTeam);
        }

        public IEnumerable<Models.Team> Get()
        {
            return _teamRepo.All().Select(i => new Models.Team() 
            {
                 Name = i.Name,
                 TeamLeadName = "Team Lead Name"
            }).AsEnumerable();
        }
    }
}
