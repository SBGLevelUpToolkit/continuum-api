using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Data.Mocks
{
    public class MockTeamRepo :  Data.ITeamRepo
    {
        public List<Data.Team> TeamData { get; private set; }

        public MockTeamRepo()
        {
            TeamData = new List<Team>();
        }

        public IEnumerable<Team> All()
        {
            return TeamData;   
        }

        public void Create(Team item)
        {
            TeamData.Add(item);
        }


        public Team FindById(int id)
        {
            return TeamData.Where(i => i.Id == id).FirstOrDefault();
        }


        public void SaveChanges()
        {
            return; 
        }

        public IEnumerable<Team> GetTeamForUser(string userId)
        {
            Func<string, IEnumerable<Data.TeamMember>, bool> userInTeam = (user, teams) => 
            {
                return teams.Any(i=>i.UserId == userId);
            };

            return TeamData.Where(i => userInTeam(userId, i.TeamMembers)).AsEnumerable();
        }


        public bool IsUserTeamAdmin(Team team, string userId)
        {
            return team.TeamMembers.Any(i => i.UserId == userId && i.IsAdmin);
        }


        public AvatarType GetAvatar(int p)
        {
            return new AvatarType() { Id = p };
        }

        public AvatarType GetDefaultAvatar()
        {
            return new AvatarType() { Id = 1 };
        }
    }
}
