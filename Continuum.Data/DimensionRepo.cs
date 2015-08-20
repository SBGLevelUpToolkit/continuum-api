using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Data
{
    public class DimensionRepo : IRepository<Data.Dimension>
    {
        private readonly Data.ContinuumDataContainer _container;

        public DimensionRepo()
        {
            _container = new ContinuumDataContainer();
        }

        public IEnumerable<Data.Dimension> All()
        {
            return _container.Dimensions.AsEnumerable();
        }

        public void Create(Dimension item)
        {
            _container.Dimensions.Add(item);
        }

        public bool DimensionExists(string dimensionName)
        {
            return _container.Dimensions.Any(i => i.Name == dimensionName);
        }

        public void SaveChanges()
        {
            int result = _container.SaveChanges();
        }

        public Dimension GetDimensionByName(string dimensionName)
        {
            return _container.Dimensions.First(i => i.Name == dimensionName);
        }

        public IEnumerable<Data.Level> CapabilityLevels()
        {
            return _container.Levels.AsEnumerable();
        }
    }
}
