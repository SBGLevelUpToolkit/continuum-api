using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Data
{
    public class TeamRepository : IRepository<Data.Team>
    {
        private readonly Data.ContinuumDataContainer _container;

        public TeamRepository()
        {
            _container = new ContinuumDataContainer();
        }

        public IEnumerable<Team> All()
        {
            return _container.Teams.AsEnumerable();
        }
    }
}
