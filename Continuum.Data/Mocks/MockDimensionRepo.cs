using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Data.Mocks
{
    public class MockDimensionRepo : IRepository<Data.Dimension>
    {
        public IEnumerable<Data.Dimension> All()
        {
            for (int i = 0; i < 12; i++)
            {
                yield return new Data.Dimension() 
                {
                    Name = "Dimension " + i,
                    Active = i % 2 == 0 ? true : false
                };
            }
            yield break;
        }

        public void Create(Dimension item)
        {
            
        }


        public Dimension FindById(int id)
        {
            throw new NotImplementedException();
        }


        public void SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
