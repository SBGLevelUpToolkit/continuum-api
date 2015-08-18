using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Repo.Models
{
    /// <summary>
    /// A dimension in the capabilitie model. For example 'Software Design'.
    /// </summary>
    public class Dimension
    {
        /// <summary>
        /// The set of Capabilities assigned to this dimension.
        /// </summary>
        public IEnumerable<Capability> Capabilities { get; set; }

        /// <summary>
        /// The name of this dimension.
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
