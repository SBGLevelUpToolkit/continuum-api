using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Data
{
    public class DimensionRepo : IRepository<Data.Dimension>
    {
        public IEnumerable<Data.Dimension> All()
        {
            Continuum.Data.ContinuumDataContainer container = new Data.ContinuumDataContainer();
            return container.Dimensions.AsEnumerable();
        }

        public void Create(Dimension item)
        {
            throw new NotImplementedException();
        }
    }
}
