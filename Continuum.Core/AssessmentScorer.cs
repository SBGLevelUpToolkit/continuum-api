using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Core
{
    public class AssessmentScorer
    {
        private IEnumerable<Tuple<int,int,int>> _dimensionCapabilityCount;

        
        public AssessmentScorer(IEnumerable<Tuple<int, int, int>> dimensionCapabilityCounts)
        {
            _dimensionCapabilityCount = dimensionCapabilityCounts;
        }

        public Models.AssessmentScoringResult CalculateScore(IEnumerable<Models.AssessmentScoringItem> assessmentItems)
        {
            var result = new Models.AssessmentScoringResult();

            if (assessmentItems == null)
            {
                result.DimensionResults = new List<Models.DimensionResult>();
            }

            result.TotalUserCount = assessmentItems.GroupBy(i => i.UserId).Count();

            var groupByDimension = assessmentItems.GroupBy(i => i.DimensionId);

            var dimensionResults = groupByDimension.Select(i => new Models.DimensionResult()
            {
                DimensionId = i.Key,
                ResponseCount = i.Count(),
                Levels = i.GroupBy(j => j.Level)
                .Select(k => new Models.LevelResult()
                {
                    Level = k.Key,
                    ResponseCount = k.Count(x=>x.CapabilityAchieved),
                    TargetCapabilityCount = GetCapabilityCountForLevel(i.Key, k.Key),
                    LevelAchieved = k.Count(x => x.CapabilityAchieved) == result.TotalUserCount * GetCapabilityCountForLevel(i.Key, k.Key)
                })
            });

            result.DimensionResults = dimensionResults;

            AddMissingDimensionsAndLevels(result);

            return result;
        }

        private void AddMissingDimensionsAndLevels(Models.AssessmentScoringResult result)
        {
            List<Models.DimensionResult> dimensions = result.DimensionResults.ToList();

            foreach (var item in _dimensionCapabilityCount)
            {
                Models.DimensionResult dimension = null;
                if (dimensions.Any(i => i.DimensionId == item.Item1))
                {
                    dimension = dimensions.First(i => i.DimensionId == item.Item1);
                }
                else
                {
                    dimension = new Models.DimensionResult() { DimensionId = item.Item1 };
                    dimensions.Add(dimension);
                }

                AddMissingLevels(dimension, item.Item2);
            }

            result.DimensionResults = dimensions;
        }

        private void AddMissingLevels(Models.DimensionResult dimension, int level)
        {
            if(dimension.Levels == null)
            {
                dimension.Levels = new List<Models.LevelResult>();
            }

            if (!dimension.Levels.Any(i => i.Level == level))
            {
                var levels = dimension.Levels.ToList();
                levels.Add(new Models.LevelResult()
                {
                    Level = level,
                    LevelAchieved = false
                });
                dimension.Levels = levels;
            }   
        }

        private int GetCapabilityCountForLevel(int dimension, int level)
        {
            try
            {
                var item =  _dimensionCapabilityCount.Where(l => l.Item1 == dimension && l.Item2 == level).First();
                if (item != null)
                {
                    return item.Item3; 
                }
                else 
                {
                    throw new ApplicationException(String.Format("No capability count has been defined for Dimension {0} and Level {1}", dimension, level));
                }
            }
            catch (InvalidOperationException)
            {
                throw new ApplicationException(String.Format("No capability count has been defined for Dimension {0} and Level {1}", dimension, level));
            }
        }
    }
}
