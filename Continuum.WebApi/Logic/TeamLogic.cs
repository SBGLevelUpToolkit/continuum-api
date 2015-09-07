using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Continuum.WebApi.Logic
{
    public class TeamLogic : LogicBase
    {
        private readonly Data.ITeamRepo _teamRepo;
    
        public TeamLogic(Data.ITeamRepo teamRepo, System.Security.Principal.IPrincipal principal) : base(principal)
        {
            if (teamRepo == null) throw new ArgumentException("Value cannot be null", "teamRepo");

            _teamRepo = teamRepo;
        }

        internal Models.Team CreateTeam(Models.Team team)
        {
            
           if (_teamRepo.All().Any(i => i.Name == team.Name.Trim()))
           {
               throw new ApplicationException(string.Format("A Team called {0} already exists.", team.Name));
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

           newTeam.TeamMembers.Add(new Data.TeamMember() { UserId = CurrentUserName, IsAdmin = true });

           _teamRepo.Create(newTeam);
           _teamRepo.SaveChanges();


           return new Models.Team()
           {
               AvatarId = newTeam.AvatarTypeId,
               Id = newTeam.Id,
               Name = newTeam.Name,
               TeamLeadName = newTeam.TeamMembers.Where(i => i.IsAdmin).First().UserId
           };
        }

        internal void JoinTeam(Models.Team team)
        {
            var teamEntry = _teamRepo.FindById(team.Id);
            if (teamEntry == null)
            {
                throw new ApplicationException("Team not found.");
            }

            string user = CurrentUserName;

            if(!teamEntry.TeamMembers.Any(i=>i.UserId == user))
            {
                teamEntry.TeamMembers.Add(new Data.TeamMember() { UserId = user, IsAdmin = false });
                _teamRepo.SaveChanges();
            }
        }

        internal IEnumerable<Models.Team> ListTeams()
        {
            return _teamRepo.All().Select(i => new Models.Team()
            {
                Name = i.Name,
                TeamLeadName = i.TeamMembers.Where(j => j.IsAdmin).FirstOrDefault().UserId
            }).AsEnumerable();
        }
    }
}