using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Data
{
    public class LookupRepo : ILookupRepo
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


        public Lookup FindById(int id)
        {
            throw new NotImplementedException();
        }


        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public T GetLookupForValue<T>(string value) where T : Lookup, new()
        {
            return _container.Lookups
                .OfType<T>()
                .Where(i => i.Value == value)
                .FirstOrDefault(); 
        }
    }
}
