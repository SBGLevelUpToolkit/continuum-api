using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Continuum.Core.Models
{
    /// <summary>
    /// Represents a user's assessment of a current capability for the team. 
    /// </summary>
    public class AssessmentItem
    {
        /// <summary>
        /// The assessment Id.
        /// </summary>
        public int AssesmentId { get; set; }

        /// <summary>
        /// The capability Id that this assessment item refers to.
        /// </summary>
        public int CapabilityId { get; set; }

        /// <summary>
        /// Has the team achieved this capability?
        /// </summary>
        public bool CapabilityAchieved { get; set; }

        public static AssessmentItem MapFrom(Data.AssessmentItem assessmentItem)
        {
            return new Models.AssessmentItem() { AssesmentId = assessmentItem.AssessmentId, CapabilityAchieved = assessmentItem.CapabilityAchieved, CapabilityId = assessmentItem.CapabiltyId };
        }
    }
}