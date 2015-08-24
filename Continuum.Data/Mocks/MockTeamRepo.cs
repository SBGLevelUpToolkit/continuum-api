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
    }
}
