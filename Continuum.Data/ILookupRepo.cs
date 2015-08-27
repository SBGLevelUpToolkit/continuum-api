using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Data
{
    public interface ILookupRepo
    {
        T GetLookupForValue<T>(string value) where T : Lookup, new();
    }
}
