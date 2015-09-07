using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Data.Mocks
{
    public class MockGoalsDBSet : MockDbSet<Data.Goal>
    {
        public MockGoalsDBSet()
        {
        }

        public override Goal Find(params object[] keyValues)
        {
            return this.Where(i => i.Id == (int)keyValues.Single()).FirstOrDefault();
        }
    }
}
