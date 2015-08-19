using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Data.Mocks
{
    public class MockTeamRepo :  IRepository<Data.Team>
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
    }
}
