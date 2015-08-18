using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Continuum.Repo.Models
{
    /// <summary>
    ///  A capability in the model.
    /// </summary>
    public class Capability
    {
        /// <summary>
        /// The description of the capability.
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// The associated level of the capability.
        /// </summary>
        [Required]
        public int Level { get; set; }

        /// <summary>
        /// The set of capabilities that must be gained before achieving this capability.
        /// </summary>
        public IEnumerable<Capability> Predecessors { get; set; }
    }
}
