using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Continuum.Core.Models;

namespace Continuum.WebApi.Logic
{
    public class AssessmentLogic : LogicBase
    {
        private readonly Data.AssessmentRepo _assessmentRepo;
        private readonly Data.TeamRepo _teamRepo;
        private readonly Data.DimensionRepo _dimensionRepo;

        public AssessmentLogic(Data.AssessmentRepo assessmentRepo, Data.TeamRepo teamRepo, Data.DimensionRepo dimensionRepo, System.Security.Principal.IPrincipal principal)
            : base(principal)
        {
            if (assessmentRepo == null) throw new ArgumentException("Value cannot be null", "assessmentRepo");
            if (teamRepo == null) throw new ArgumentException("Value cannot be null", "teamRepo");
            if (dimensionRepo == null) throw new ArgumentException("Value cannot be null", "dimensionRepo");

            _assessmentRepo = assessmentRepo;
            _teamRepo = teamRepo;
        }

        public bool AssessmentIsAvailable()
        {
            var team = GetTeamForCurrentUser();
            var assessment = _assessmentRepo.GetCurrentAssessmentForTeam(team.Id);
            return assessment != null;
        }

        internal Core.Models.Assessment GetAssessment()
        {
            var team = GetTeamForCurrentUser();

            var assessment = GetCurrentAssessmentForTeam(team);

            List<Core.Models.AssessmentItem> assessmentItems = null;
            Core.Models.AssessmentScoringResult assessmentScoringResult = null;

            if (assessment.Status.Value == "Open")
            {
                assessmentItems = _assessmentRepo.GetCurrentAssessmentItemsForUser(CurrentUserName).Select(i => AssessmentItem.MapFrom(i)).ToList();
            }

            var assessmentResults = assessment.AssessmentResults.Select(i=>  Core.Models.AssessmentResult.MapFrom(i));

            var result = new Core.Models.Assessment()
            {
                Id = assessment.Id,
                Status = assessment.Status.Value,
                AssessmentItems = assessmentItems,
                AssessmentResults = assessmentResults
            };

            return result;
        }

        public Core.Models.AssessmentScoringResult ScoreCurrentAssessment()
        {
            var team = GetTeamForCurrentUser();

            var assessment = GetCurrentAssessmentForTeam(team);

            var assessmentScoringItems = _assessmentRepo.GetAssessmentItems(assessment.Id).Select(i => new Core.Models.AssessmentScoringItem()
            {
                AssesmentId = assessment.Id,
                UserId = i.TeamMemberId.ToString(),
                Level = i.Capabilty.LevelId,
                CapabilityAchieved = i.CapabilityAchieved,
                CapabilityId = i.CapabiltyId,
                DimensionId = i.Capabilty.DimensionId
            });

            var levels = _dimensionRepo.GetCapabilitiesPerLevel();
            Core.AssessmentScorer scorer = new Core.AssessmentScorer(levels);

            var result = scorer.CalculateScore(assessmentScoringItems);
            return result;
        }

        private Data.Assessment GetCurrentAssessmentForTeam(Data.Team team)
        {
            var assessment = _assessmentRepo.GetCurrentAssessmentForTeam(team.Id);

            if (assessment == null)
            {
                throw new ApplicationException("No assessment data is available for this team.");
            }
            return assessment;
        }

        private Data.Team GetTeamForCurrentUser()
        {
            var team = _teamRepo.GetTeamForUser(CurrentUserName).FirstOrDefault();

            if (team == null)
            {
                throw new ApplicationException(String.Format("'{0}' is not a member of a team.", CurrentUserName));
            }
            return team;
        }

        internal void UpdateAssessmentResults(IEnumerable<AssessmentResult> assessmentResults)
        {
            var team = GetTeamForCurrentUser();
            var assessment = GetCurrentAssessmentForTeam(team);

            _assessmentRepo.UpdateModerationResultForAssessment(assessment, assessmentResults.Select(i =>
                new Data.AssessmentResult()
                {
                    AssessmentId = i.AssessmentId,
                    DimensionId = i.DimensionId,
                    Rating = i.Rating.ToString()
                }));
            
            _assessmentRepo.SaveChanges(); 
        }

        internal void UpdateAssessmentItems(IEnumerable<AssessmentItem> assessmentItems)
        {
            var team = GetTeamForCurrentUser();
            var assessment = GetCurrentAssessmentForTeam(team);
            var teamMember = team.TeamMembers.Where(i => i.UserId == CurrentUserName).First();

            _assessmentRepo.UpdateCapabilityForAssessment(assessment,
                assessmentItems.Select(i => new Data.AssessmentItem()
                {
                    AssessmentId = i.AssesmentId,
                    CapabilityAchieved = i.CapabilityAchieved,
                    CapabiltyId = i.CapabilityId,
                    TeamMemberId = teamMember.Id
                }));

            _assessmentRepo.SaveChanges();
        }

        internal void CreateAssessment()
        {
            var team = GetTeamForCurrentUser();
            _assessmentRepo.CreateAssessmentForTeam(team);
            _assessmentRepo.SaveChanges();
        }

        internal void ModerateAssessment()
        {
            var team = GetTeamForCurrentUser();
            var assessment = GetCurrentAssessmentForTeam(team);

            _assessmentRepo.StartModeration(assessment);
            _assessmentRepo.SaveChanges();          
        }

        internal void CloseAssessment()
        {
            
           var team = GetTeamForCurrentUser();
           var assessment = GetCurrentAssessmentForTeam(team);

           _assessmentRepo.CloseAssessment(assessment);
           _assessmentRepo.SaveChanges(); 
        }

        internal void ReopenAssessment()
        {

            var team = GetTeamForCurrentUser();
            var assessment = GetCurrentAssessmentForTeam(team);

            _assessmentRepo.ReopenAssessment(assessment);
            _assessmentRepo.SaveChanges();
        }
    }
}