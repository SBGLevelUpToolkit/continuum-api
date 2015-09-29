using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Data.Mocks
{
    public class MockTeamMembersDBSet : MockDbSet<Data.TeamMember>
    {

        public MockTeamMembersDBSet()
        {
            var team = new Data.Team()
            {
                Id = 1,
                Name = "Test Team",
                AvatarType = new AvatarType() { Value = "Avatar" }
            };

            var teamMember = new TeamMember() { UserId = "alice@example.com", IsAdmin = true, Team = team, TeamId = team.Id };

            team.TeamMembers.Add(teamMember);

            Add(teamMember);
        }

        public override TeamMember Find(params object[] keyValues)
        {
            return this.Where(i => i.Id == (int)keyValues.Single()).FirstOrDefault();  
        }

    }
}
