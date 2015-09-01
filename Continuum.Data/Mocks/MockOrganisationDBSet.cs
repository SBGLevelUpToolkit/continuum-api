using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Data.Mocks
{
    public class MockOrganisationDBSet : MockDbSet<Data.Organisation>
    {
        public MockOrganisationDBSet()
        {
            this.Add(new Organisation() { Name = "Standard Bank" });
        }
    }
}
