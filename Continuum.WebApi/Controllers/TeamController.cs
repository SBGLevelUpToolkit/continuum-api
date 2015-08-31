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
        private readonly Data.ITeamRepo _teamRepo;

        public TeamController(Data.ITeamRepo teamRepo)
        {
            _teamRepo = teamRepo;
        }

        public HttpResponseMessage Put(Models.Team team)
        {
            if (_teamRepo.All().Any(i => i.Name == team.Name.Trim()))
            {
                throw ExceptionBuilder.CreateInternalServerError(string.Format("A Team called {0} already exists.", team.Name), "Duplicate Team");
            }


            Data.AvatarType avatar = null;
            if(team.AvatarId > 0)
            {
                avatar = _teamRepo.GetAvatar(team.AvatarId);
            }
            else
            {
                avatar = _teamRepo.GetDefaultAvatar();
            }

            Data.Team newTeam = new Data.Team()
            {
                Name = team.Name,
                AvatarType = avatar
            };

            newTeam.TeamMembers.Add(new Data.TeamMember() { UserId = this.User.Identity.Name, IsAdmin = true });

            _teamRepo.Create(newTeam);
            _teamRepo.SaveChanges();


            var resultObject = new Models.Team()
            {
                AvatarId = newTeam.AvatarTypeId,
                Id = newTeam.Id,
                Name = newTeam.Name,
                TeamLeadName = newTeam.TeamMembers.Where(i => i.IsAdmin).First().UserId
            };

            var response = new HttpResponseMessage(HttpStatusCode.Created);
            response.Content = new ObjectContent(typeof(Models.Team), resultObject, new JsonMediaTypeFormatter());

            return response;
        }

        public void Post(Models.Team team)
        {
            var teamEntry = _teamRepo.FindById(team.Id);
            if (teamEntry == null)
            {
                throw ExceptionBuilder.CreateException("Team not found.", "Not Found", HttpStatusCode.NotFound);
            }

            string user = this.User.Identity.Name;

            if(!teamEntry.TeamMembers.Any(i=>i.UserId == user))
            {
                teamEntry.TeamMembers.Add(new Data.TeamMember() { UserId = this.User.Identity.Name, IsAdmin = false });
                _teamRepo.SaveChanges();
            }

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
