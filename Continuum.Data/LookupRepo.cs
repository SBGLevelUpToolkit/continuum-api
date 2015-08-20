using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Data
{
    public class LookupRepo : IRepository<Data.Lookup>
    {
        private readonly ContinuumDataContainer _container;

        public LookupRepo()
        {
            _container = new ContinuumDataContainer();
        }

        public IEnumerable<Lookup> All()
        {
            return _container.Lookups.AsEnumerable();
        }

        public void Create(Lookup item)
        {
           
        }
    }
}
