using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Data
{
    public abstract class RepositoryBase : IRepository
    {
        protected readonly IContinuumDataContainer _container; 

        public RepositoryBase(IContinuumDataContainer container)
        {
            _container = container;
        }

        protected virtual void BeforeSaveChanges(){
            return;
        }

        protected virtual void AfterSaveChanges()
        {
            return;
        }

        public void SaveChanges()
        {
            BeforeSaveChanges();
            _container.SaveChanges();
            AfterSaveChanges();        
        }
    }
}
