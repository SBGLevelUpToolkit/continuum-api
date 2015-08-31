using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Continuum.WebApi.Models
{
    public class AssessmentResult
    {
        public int AssessmentId { get; set; }
        public int DimensionId { get; set; }
        public int Rating { get; set; }
    }
}