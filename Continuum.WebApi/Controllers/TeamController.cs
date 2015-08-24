using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Routing;

namespace Continuum.WebApi.Controllers
{
    [Authorize]
    public class TeamController : ApiController
    {
        private readonly Data.IRepository<Data.Team> _teamRepo;

        public TeamController(Data.IRepository<Data.Team> teamRepo)
        {
            _teamRepo = teamRepo;
        }

        public HttpResponseMessage Put(Models.Team team)
        {
            if (_teamRepo.All().Any(i => i.Name == team.Name.Trim()))
            {
                throw ExceptionBuilder.CreateInternalServerError(string.Format("A Team called {0} already exists.", team.Name), "Duplicate Team");
            }

            Data.Team newTeam = new Data.Team()
            {
                Name = team.Name
            };

            newTeam.TeamMembers.Add(new Data.TeamMember() { UserId = this.User.Identity.Name, IsAdmin = true });

            _teamRepo.Create(newTeam);

            var response = new HttpResponseMessage(HttpStatusCode.Created);
            response.Content = new ObjectContent(typeof(Data.Team), newTeam, new JsonMediaTypeFormatter());

            string uri = Url.Route("Default",  new { controller = "Team", id = newTeam.Id });
            response.Headers.Location = new Uri(uri);   
         
            return response;
        }

        public IEnumerable<Models.Team> Get()
        {
            return _teamRepo.All().Select(i => new Models.Team() 
            {
                 Name = i.Name,
                 TeamLeadName = i.TeamMembers.Where(j=>j.IsAdmin).FirstOrDefault().UserId
            }).AsEnumerable();
        }
       
    }
}
