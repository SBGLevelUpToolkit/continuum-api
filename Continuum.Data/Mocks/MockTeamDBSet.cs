using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Data.Mocks
{
    public class MockTeamDBSet : MockDbSet<Data.Team>
    {
        public MockTeamDBSet()
        {

            var team = new Data.Team()
            {
                Id = 1,
                Name = "Test Team",
                AvatarType = new AvatarType() { Value = "Avatar" }
            };

            team.TeamMembers.Add(new TeamMember(){ UserId = "alice@example.com", IsAdmin = true, Team = team});

            this.Add(team);
        }

        public override Team Find(params object[] keyValues)
        {
            return this.Where(i => i.Id == (int)keyValues.Single()).FirstOrDefault();        
        }
    }
}
