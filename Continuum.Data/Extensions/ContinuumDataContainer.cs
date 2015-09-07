using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Data
{
    public partial class ContinuumDataContainer : IContinuumDataContainer
    {
        public void SetStateForEntity<T>(T entity, System.Data.Entity.EntityState state) where T : class
        {
            this.Entry(entity).State = state;        
        }
    }
}
