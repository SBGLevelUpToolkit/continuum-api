using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Data.Mocks
{
    public class MockLookupDBSet : MockDbSet<Data.Lookup>
    {
        public MockLookupDBSet()
        {
            this.Add(new Data.AssessmentStatus() { Value = "Open" });
            this.Add(new Data.AssessmentStatus() { Value = "Closed" });
            this.Add(new Data.AssessmentStatus() { Value = "Moderating" });
        }
    }
}
