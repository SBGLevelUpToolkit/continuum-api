using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Continuum.WebApi.Logic
{
    public class AssessmentLogic
    {
        private readonly Data.IAssessmentRepo _assessmentRepo;
        private readonly Data.ITeamRepo _teamRepo;
        private readonly System.Security.Principal.IPrincipal _principal;

        public AssessmentLogic(Data.IAssessmentRepo assessmentRepo, Data.ITeamRepo teamRepo, System.Security.Principal.IPrincipal principal)
        {
            if (principal == null) throw new ArgumentException("Value cannot be null", "principal");
            if (assessmentRepo == null) throw new ArgumentException("Value cannot be null", "assessmentRepo");
            if (teamRepo == null) throw new ArgumentException("Value cannot be null", "teamRepo");

            _assessmentRepo = assessmentRepo;
            _teamRepo = teamRepo;
            _principal = principal;
        }

        private string CurrentUserName
        {
            get
            {
                return _principal.Identity.Name; 
            }
        }

        internal Models.Assessment GetAssessment()
        {
            var team = _teamRepo.GetTeamForUser(CurrentUserName).FirstOrDefault();

            if (team == null)
            {
                throw new ApplicationException(String.Format("{0} is not a member of a team.", CurrentUserName));
            }

            var assessment = _assessmentRepo.GetCurrentAssessmentForTeam(team.Id);

            if (assessment == null)
            {
                throw new ApplicationException("No assessment data is available for this team.");
            }

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
    }
}