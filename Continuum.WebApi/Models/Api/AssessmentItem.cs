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

        public static AssessmentItem MapFrom(Data.AssessmentItem assessmentItem)
        {
            return new Models.AssessmentItem() { AssesmentId = assessmentItem.AssessmentId, CapabilityAchieved = assessmentItem.CapabilityAchieved, CapabilityId = assessmentItem.CapabiltyId };
        }
    }
}