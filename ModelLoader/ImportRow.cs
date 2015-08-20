using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLoader
{
    public class ImportRow
    {
        public string Dimension { get; set; }

        public string Level { get; set; }

        public string TempId { get; set; }

        public string Description { get; set; }

        public string[] Predecessors { get; set; }

        internal bool IsValid()
        {
            return String.IsNullOrEmpty(Dimension) == false
                && String.IsNullOrEmpty(Level) == false
                && String.IsNullOrEmpty(TempId) == false
                && String.IsNullOrEmpty(Description) == false; 
        }
    }
}
