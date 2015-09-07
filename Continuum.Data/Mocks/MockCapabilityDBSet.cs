using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Data.Mocks
{
    public class MockCapabilityDBSet : MockDbSet<Data.Capability>
    {
        public override Capability Find(params object[] keyValues)
        {
            return this.First(i => i.Id == (int)keyValues.Single());
        }
    }
}
