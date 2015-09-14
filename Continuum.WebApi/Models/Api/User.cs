using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Continuum.WebApi.Models
{
    public class User
    {
        public string UserId { get; set; }
        public IEnumerable<Team> Teams{ get; set; }

        public bool IsAdmin { get; set; }
    }
}