using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Continuum.Core.Models
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

    public class AssessmentScoringResult
    {
        public IEnumerable<DimensionResult> DimensionResults { get; set; }

        /// <summary>
        /// The number of unique users who provided assessment results.
        /// </summary>
        public int TotalUserCount { get; set; }
    }

    public class DimensionResult
    {
        public int DimensionId { get; set; }
        public int Rating { get; set; }

        /// <summary>
        /// The number of responses for this dimension.
        /// </summary>
        public int ResponseCount { get; set; }

        public IEnumerable<LevelResult> Levels { get; set; }
    }

    public class LevelResult
    {
        public int Level { get; set; }

        public int ResponseCount { get; set; }

        /// <summary>
        /// The number of capabilities defined for this level.
        /// </summary>
        public int TargetCapabilityCount;

        public bool LevelAchieved { get; set; }
    }

    public class CapabilityResult
    {
        public int CapabilityId { get; set; }

        public int ResultCount { get; set; }
    }
}