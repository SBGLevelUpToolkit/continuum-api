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
        private static AssessmentScorer CreateScorer()
        {
            var capabilityMap = new List<Tuple<int, int, int>>();

            capabilityMap.Add(new Tuple<int, int, int>(1, 1, 2));
            capabilityMap.Add(new Tuple<int, int, int>(1, 2, 3));
            capabilityMap.Add(new Tuple<int, int, int>(2, 1, 1));

            AssessmentScorer scorer = new AssessmentScorer(capabilityMap);
            return scorer;
        }

        [TestMethod]
        public void GivenAnAssessmentWithTwoResultsForSameDimensionThenReturnOnlyOneResult()
        {
            var assessmentItems = CreateAssessmentWithResponses();


            AssessmentScorer scorer = CreateScorer();

            var result = scorer.CalculateScore(assessmentItems);

            Assert.IsTrue(result.DimensionResults.Where(i=>i.DimensionId == 1).Count() == 1, "Only one result per dimension must be returned.");
            Assert.IsTrue(result.DimensionResults.First().DimensionId == 1, "Incorrect dimension id was returned.");
        }

        private static IEnumerable<Core.Models.AssessmentScoringItem> CreateAssessmentWithResponses()
        {
            var assessmentItems = new List<Core.Models.AssessmentScoringItem>();
            assessmentItems.Add(new Core.Models.AssessmentScoringItem() { DimensionId = 1 , CapabilityId = 1, Level = 1, UserId = "Bob", CapabilityAchieved=true});
            assessmentItems.Add(new Core.Models.AssessmentScoringItem() { DimensionId = 1, CapabilityId = 2, Level = 1, UserId = "Fred", CapabilityAchieved = true });

            return assessmentItems;
        }

        [TestMethod]
        public void ResultCountPerDimensionMustEqualNumberOfResponses()
        {
            var assessmentItems = CreateAssessmentWithResponses();

            AssessmentScorer scorer = CreateScorer();

            var result = scorer.CalculateScore(assessmentItems);

            Assert.IsTrue(result.DimensionResults.First().ResponseCount == assessmentItems.Count());
            
        }

        [TestMethod]
        public void ResultCountPerLevelMustEqualNumberOfResponsesPerLevel()
        {
            var assessmentItems = CreateAssessmentWithResponses();

            AssessmentScorer scorer = CreateScorer();

            var result = scorer.CalculateScore(assessmentItems);

            Assert.IsTrue(result.DimensionResults.First().Levels.First().ResponseCount == 2);
            Assert.IsTrue(result.DimensionResults.First().Levels.First().Level == 1);
        }

        [TestMethod]
        public void TotalUserCountMustEqualNumberOfUniqueUsers()
        {
            var assessmentItems = CreateAssessmentWithResponses();

            AssessmentScorer scorer = CreateScorer();

            var result = scorer.CalculateScore(assessmentItems);

            Assert.IsTrue(result.TotalUserCount == 2, "Incorrect number of Users.");
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void GivenMissingCapabilityMapThenExceptionMustBeThrown()
        {
             var assessmentItems = CreateAssessmentWithResponses();

             var capabilityMap = new List<Tuple<int, int, int>>();
            AssessmentScorer scorer = new AssessmentScorer(capabilityMap);

            var result = scorer.CalculateScore(assessmentItems);

            Assert.IsTrue(result.DimensionResults.First().Levels.First().Level == 1);
        }

        [TestMethod]
        public void CorrectTargetCapabilityCountMustBeSetOnResult()
        {
          var assessmentItems = CreateAssessmentWithResponses();

            AssessmentScorer scorer = CreateScorer();

            var result = scorer.CalculateScore(assessmentItems);

            Assert.IsTrue(result.DimensionResults.First().Levels.First().TargetCapabilityCount == 2, "Incorrect capability count.");   
        }

        [TestMethod]
        public void LevelAchievedMustNotBeSetIfNotAllUsersMarkedAllCapabilitiesForALevel()
        {
             var assessmentItems = CreateAssessmentWithResponses();

            AssessmentScorer scorer = CreateScorer();

            var result = scorer.CalculateScore(assessmentItems);

            Assert.IsTrue(result.DimensionResults.First().Levels.First().LevelAchieved == false, "Level was not achieved."); 
        }

        [TestMethod]
        public void LevelAchievedMustBeSetIfAllUsersMarkedAllCapabilitiesForALevel()
        {
            var assessmentItems = CreateAssessmentWithResponses();

            var items = assessmentItems as List<Core.Models.AssessmentScoringItem>;
            items.Add(new Core.Models.AssessmentScoringItem() { DimensionId = 1, CapabilityId = 2, Level = 1, UserId = "Bob", CapabilityAchieved = true });
            items.Add(new Core.Models.AssessmentScoringItem() { DimensionId = 1, CapabilityId = 1, Level = 1, UserId = "Fred", CapabilityAchieved = true });

            AssessmentScorer scorer = CreateScorer();

            var result = scorer.CalculateScore(items);

            Assert.IsTrue(result.DimensionResults.First().Levels.First().LevelAchieved == true, "Level should be achieved.");
        }

        [TestMethod]
        public void TestThatMissingDimensionsAreAdded()
        {
            var assessmentItems = CreateAssessmentWithResponses();

            AssessmentScorer scorer = CreateScorer();

            var result = scorer.CalculateScore(assessmentItems);

            Assert.IsTrue(result.DimensionResults.Where(i=>i.DimensionId == 2).Any(), "The missing dimension was not added.");
            Assert.IsTrue(result.DimensionResults.Where(i => i.DimensionId == 2).First().Levels.Count() == 1, "The missing level was not added.");  
        }
    }
}
