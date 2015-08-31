using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Continuum.WebApi.Models
{
    public class AssessmentItem
    {
        public int AssesmentId { get; set; }

        public int CapabilityId { get; set; }

        public bool CapabilityAchieved { get; set; }
    }
}