using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Core.Models
{
    public class TeamMember
    {
        public string UserId { get; set; }

        public bool IsAdmin { get; set; }

        public string UserName { get; set; }
    }
}
