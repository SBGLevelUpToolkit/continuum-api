using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.WebApi.Models
{
    public class Goal
    {
        public int Id { get; set; }

        public int DimensionId { get; set; }

        public int CapabilityId { get; set; }

        public string Notes { get; set; }

        public DateTime DueDate { get; set; }

        public bool Completed { get; set; }

    }
}
