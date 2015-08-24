using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Continuum.WebApi.Controllers
{
    public class UserController : ApiController
    {
        private readonly Data.IRepository<Data.Team> _teamRepo; 

        public UserController(Data.IRepository<Data.Team> teamRepository)
        {
            _teamRepo = teamRepository;
        }

        public void Put(Models.User user)
        {
            if(user.UserId != User.Identity.Name){
                throw ExceptionBuilder.CreateException("You may not update another user's details", "Permission Denied", HttpStatusCode.Forbidden);
            }

            foreach(var userTeam in user.Teams)
            {
                var team = _teamRepo.FindById(userTeam.Id);
                if (team != null)
                {
                    if (team.TeamMembers.Any(i => i.UserId == user.UserId) == false)
                    {
                        team.TeamMembers.Add(new Data.TeamMember() { UserId = user.UserId });
                    }
                }
                else
                {
                    throw ExceptionBuilder.CreateInternalServerError(string.Format("{0} is not a valid team id.", userTeam.Id), "Team not found.");
                }
            }

            _teamRepo.SaveChanges();
        }
    }
}
