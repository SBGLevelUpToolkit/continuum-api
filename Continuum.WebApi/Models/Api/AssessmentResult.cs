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

        public static AssessmentResult MapFrom(Data.AssessmentResult result)
        {
            return new Models.AssessmentResult() { AssessmentId = result.AssessmentId, DimensionId = result.DimensionId, Rating = Int32.Parse(result.Rating) };
        }
    }
}