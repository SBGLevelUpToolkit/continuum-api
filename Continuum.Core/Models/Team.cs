using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Core.Models
{
    public class Team
    {
        [Required(ErrorMessage="The name field is required.")]
        public string Name { get; set; }

        public string TeamLeadName { get; set; }

        public string AvatarName { get; set; }

        public int Id { get; set; }
    }
}
