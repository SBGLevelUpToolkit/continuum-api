using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Continuum.Data;

namespace Continuum.WebApi.Controllers
{
    public class UserController : ApiController
    {
        private readonly ITeamRepo _teamRepo; 

        public UserController(ITeamRepo teamRepository)
        {
            _teamRepo = teamRepository;
        }

        public Models.Team Get()
        {
            var team = _teamRepo.GetTeamForUser(User.Identity.Name).FirstOrDefault();
            if (team == null)
            {
                team = new Team() { Name = string.Format("{0} is not a member of any team.", User.Identity.Name) };
            }

            string adminName = team.TeamMembers.Where(i => i.IsAdmin).FirstOrDefault().UserId;

            return new Models.Team { Name = team.Name, Id = team.Id, AvatarId = team.AvatarTypeId, TeamLeadName = adminName };
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
