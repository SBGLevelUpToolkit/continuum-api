using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Core.Models
{
    public class Assessment
    {
        public int Id { get; set; }

        /// <summary>
        /// The status of the assessment.
        /// </summary>
        /// <example>
        /// Open, Closed or Moderating
        /// </example>
        public string Status { get; set; }

        public IEnumerable<Models.AssessmentItem> AssessmentItems { get; set; }

        public IEnumerable<Models.AssessmentResult> AssessmentResults { get; set; }
    }
}
