using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using Continuum.Core;
using Continuum.Core.Models; 

namespace Continuum.WebApi.Logic
{
    public class TeamLogic : LogicBase
    {
        private readonly Data.TeamRepo _teamRepo;

        public TeamLogic(Data.TeamRepo teamRepo, System.Security.Principal.IPrincipal principal)
            : base(principal)
        {
            if (teamRepo == null) throw new ArgumentException("Value cannot be null", "teamRepo");

            _teamRepo = teamRepo;
        }

        public Team CreateTeam(Team team)
        {
            
           if (_teamRepo.All().Any(i => i.Name == team.Name.Trim()))
           {
               throw new ApplicationException(string.Format("A Team called {0} already exists.", team.Name));
           }


           Data.AvatarType avatar = null;
           if(!String.IsNullOrEmpty(team.AvatarName))
           {
               avatar = _teamRepo.GetAvatar(team.AvatarName);
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


           return new Team()
           {
               AvatarName = newTeam.AvatarType.Value,
               Id = newTeam.Id,
               Name = newTeam.Name,
               TeamLeadName = newTeam.TeamMembers.Where(i => i.IsAdmin).First().UserId
           };
        }

        internal void JoinTeam(Team team)
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

        internal IEnumerable<Team> ListTeams()
        {
            return _teamRepo.All().Select(i => new Team()
            {
                Name = i.Name,
                TeamLeadName = GetAdminName(i.TeamMembers),
                AvatarName = i.AvatarType.Value,
                Id = i.Id
            }).AsEnumerable();
        }

        private string GetAdminName(IEnumerable<Data.TeamMember> teamMembers)
        {
            var admin = teamMembers.Where(i => i.IsAdmin).FirstOrDefault();
            if(admin != null)
            {
                return admin.UserId;
            }
            else
            {
                return "No Admin";
            }
        }

        internal bool TeamExists(int id)
        {
            return _teamRepo.FindById(id) != null;
        }

        internal Team GetTeam(int id)
        {
            var team = _teamRepo.FindById(id);
            if(team != null)
            {
                return new Team()
                {
                    Name = team.Name,
                    TeamLeadName = team.TeamMembers.Where(j => j.IsAdmin).FirstOrDefault().UserId,
                    AvatarName = team.AvatarType.Value,
                    Id = team.Id, 
                    TeamMembers = team.TeamMembers.Select(i=> i.UserId).ToArray()
                };
            }
            else
            {
                throw new ApplicationException("Team not found");
            }
        }

        internal Team GetTeamForUser()
        {
            var team = _teamRepo.GetTeamForUser(CurrentUserName).FirstOrDefault();
            if (team == null)
            {
                return new Team() { Name = string.Format("{0} is not a member of any team.", CurrentUserName) };
            }

            string adminName = team.TeamMembers.Where(i => i.IsAdmin).FirstOrDefault().UserId;

            return new Team { Name = team.Name, Id = team.Id, AvatarName = team.AvatarType.Value, TeamLeadName = adminName };
             
        }

        public IEnumerable<Core.Models.TeamMember> GetTeamMembers(int id)
        {
            var team = _teamRepo.FindById(id);
            if (team != null)
            {
                return team.TeamMembers.Select(i => new Core.Models.TeamMember()
                {
                    Id = i.Id,
                    IsAdmin = i.IsAdmin,
                    EmailAddress = i.UserId
                });
            }
            else
            {
                throw new ApplicationException("Team not found");
            }
        }

        internal void UpdateUser(User user)
        {
            
            if(user.UserId != CurrentUserName){
                throw new SecurityException("You may not update another user's details.");
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
                    throw new ApplicationException(string.Format("{0} is not a valid team id.", userTeam.Id));
                }
            }

            _teamRepo.SaveChanges();
        }
        public IEnumerable<Avatar> GetAvatars(){
            return _teamRepo.ListAvatars().Select(i => new Avatar() { Name = i.Value });
        }

        public User GetUserDetails(string userId)
        {
            var team = _teamRepo.GetTeamForUser(userId).FirstOrDefault();

            bool isAdmin = false;
            if(team != null)
            {
                isAdmin = team.TeamMembers.Any(i => i.IsAdmin && i.UserId == userId);
            }

            return new User()
            {
                UserId = userId,
                IsAdmin = isAdmin,
                Teams = GetTeamsForUser(userId),
            };
        }

        public IEnumerable<Team> GetTeamsForUser(string userId)
        {
            var team = _teamRepo.GetTeamForUser(userId);
            return team.Select(i => new Team()
            {
                Name = i.Name,
                Id = i.Id,
                AvatarName = i.AvatarType.Value,
                TeamLeadName = i.TeamMembers.First(j => j.IsAdmin).UserId
            });
        }

        public void DeleteTeam(int id)
        {
            if (_principal.IsInRole("SiteAdmin"))
            {
                if (TeamExists(id)) 
                {
                    _teamRepo.DeleteTeam(id);
                    _teamRepo.SaveChanges();
            }
                else
                {
                    throw new ApplicationException("Team does not exist.");
                }
            }
            else
            {
                throw new System.Security.SecurityException(CurrentUserName + "is not authorised to delete teams.");
            }
        }

        public void UpdateTeam(Core.Models.Team team)
        {
            var current = _teamRepo.FindById(team.Id);
            if (current != null)
            {
                current.Name = team.Name;

                if (!String.IsNullOrEmpty(team.AvatarName))
                {
                    current.AvatarType = _teamRepo.GetAvatar(team.AvatarName);
                }
                else
                {
                    current.AvatarType = _teamRepo.GetDefaultAvatar();
                }

                _teamRepo.SaveChanges();
            }
            else
            {
                throw new ApplicationException("Invalid team Id.");
            }
        }

        internal bool TeamMemberExists(int memberId)
        {
            return _teamRepo.TeamMemberExists(memberId);
        }

        public void DeleteTeamMember(int teamId, int memberId)
        {
            var team = _teamRepo.FindById(teamId);

            int adminCount = team.TeamMembers.Where(i=>i.IsAdmin && i.Id != memberId).Count();

            if(adminCount == 0)
            {
                throw new ApplicationException("You may not delete the last administrator in a group.");
            }
 
            _teamRepo.DeleteTeamMember(memberId);
            _teamRepo.SaveChanges();
        }
    }
}