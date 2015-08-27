using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Data
{
    public partial class Lookup
    {
        public override bool Equals(object obj)
        {
            var lookup = obj as Lookup;
            if (lookup != null)
            {
                return lookup.Value == this.Value && obj.GetType() == this.GetType(); 
            }
            else
            {
                return base.Equals(obj);
            }
        }
    }
}
