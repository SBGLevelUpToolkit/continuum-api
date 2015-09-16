using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Core.Models
{
    /// <summary>
    /// A dimension in the capabilitie model. For example 'Software Design'.
    /// </summary>
    public class Dimension
    {

        public int Id { get; set; }

        /// <summary>
        /// The set of Capabilities assigned to this dimension.
        /// </summary>
        public IEnumerable<Capability> Capabilities { get; set; }

        /// <summary>
        /// The name of this dimension.
        /// </summary>
        [Required]
        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public string ImageName { get; set; }
    }
}
