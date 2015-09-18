﻿using System;
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
                    ResponseCount = k.Count(),
                    TargetCapabilityCount = GetCapabilityCountForLevel(i.Key, k.Key),
                    LevelAchieved = k.Count() == result.TotalUserCount *  GetCapabilityCountForLevel(i.Key, k.Key)
                })
            });

            result.DimensionResults = dimensionResults;

            return result; 
        }

        private int GetCapabilityCountForLevel(int dimension, int level)
        {
            try
            {
                return _dimensionCapabilityCount.Where(l => l.Item1 == dimension && l.Item2 == level).First().Item3;
            }
            catch (InvalidOperationException)
            {
                throw new ApplicationException(String.Format("No capability count has been defined for Dimension {0} and Level {1}", dimension, level));
            }
        }
    }
}