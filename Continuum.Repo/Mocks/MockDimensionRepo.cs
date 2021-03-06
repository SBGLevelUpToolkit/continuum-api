﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Repo.Mocks
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
    }
}
