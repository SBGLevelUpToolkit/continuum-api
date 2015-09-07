using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Continuum.WebApi.Logic
{
    public class AssessmentLogic : LogicBase
    {
        private readonly Data.AssessmentRepo _assessmentRepo;
        private readonly Data.TeamRepo _teamRepo;

        public AssessmentLogic(Data.AssessmentRepo assessmentRepo, Data.TeamRepo teamRepo, System.Security.Principal.IPrincipal principal)
            : base(principal)
        {
            if (assessmentRepo == null) throw new ArgumentException("Value cannot be null", "assessmentRepo");
            if (teamRepo == null) throw new ArgumentException("Value cannot be null", "teamRepo");

            _assessmentRepo = assessmentRepo;
            _teamRepo = teamRepo;
        }

        internal Models.Assessment GetAssessment()
        {
            var team = GetTeamForCurrentUser();

            var assessment = GetCurrentAssessmentForTeam(team);

            var assessmentItems = _assessmentRepo.GetCurrentAssessmentItemsForUser(CurrentUserName).Select(i => Models.AssessmentItem.MapFrom(i));
            var assessmentResults = assessment.AssessmentResults.Select(i => Models.AssessmentResult.MapFrom(i));

            var result = new Models.Assessment()
            {
                Id = assessment.Id,
                Status = assessment.Status.Value,
                AssessmentItems = assessmentItems,
                AssessmentResults = assessmentResults
            };

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

        internal void UpdateAssessmentResults(IEnumerable<Models.AssessmentResult> assessmentResults)
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

        internal void UpdateAssessmentItems(IEnumerable<Models.AssessmentItem> assessmentItems)
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
    }
}