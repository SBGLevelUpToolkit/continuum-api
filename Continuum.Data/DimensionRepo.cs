using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Data
{
    public class DimensionRepo : IRepository
    {
        private readonly Data.IContinuumDataContainer _container;

        public DimensionRepo()
        {
            _container = new ContinuumDataContainer();
        }

        public DimensionRepo(Data.IContinuumDataContainer container)
        {
            _container = container;
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

        public Dimension FindById(Int32 id)
        {
            return _container.Dimensions.Find(id);
        }

        public IEnumerable<Data.Level> CapabilityLevels()
        {
            return _container.Levels.AsEnumerable();
        }

        public void CreateLevel(string level, string imageName)
        {
            _container.Levels.Add(new Data.Level() { DisplayName = level, ImageName = imageName});
        }

        public Capability FindCapabilityById(int id)
        {
            return _container.Capabilities.Find(id);
        }

        public IEnumerable<Tuple<int, int, int>> GetCapabilitiesPerLevel()
        {
            var levels = _container.Capabilities.GroupBy(i=> new {i.DimensionId, i.LevelId}).AsEnumerable();

            if(levels == null || levels.Count() == 0)
            {
                throw new ApplicationException("Level error");
            }

            return levels.Select(j => new Tuple<int, int, int>(j.Key.DimensionId, j.Key.LevelId, j.Count()));
        }
    }
}
