﻿using System;
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

        internal Team CreateTeam(Team team)
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
                TeamLeadName = i.TeamMembers.Where(j => j.IsAdmin).FirstOrDefault().UserId,
                AvatarName = i.AvatarType.Value,
                Id = i.Id
            }).AsEnumerable();
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
            return new User()
            {
                UserId = userId,
                IsAdmin = team.TeamMembers.Any(i => i.IsAdmin && i.UserId == userId),
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
    }
}