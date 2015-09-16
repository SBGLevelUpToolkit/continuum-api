using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Continuum.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Continuum.Tests
{
    [TestClass]
    public class AssessmentScoringTests
    {
        [TestMethod]
        public void GivenAssessmentWithNoResultsThenReturnNoScores()
        {
            Continuum.Core.Models.Assessment assessment = new Core.Models.Assessment();

            AssessmentScorer scorer = new AssessmentScorer();

            var result = scorer.CalculateScore(assessment.AssessmentItems);
    
        }
    }
}
