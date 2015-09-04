using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Data
{
    public class AssessmentRepo : IAssessmentRepo
    {
        protected readonly IContinuumDataContainer _container;
        public AssessmentRepo()
        {
            _container = new ContinuumDataContainer();
        }

        public AssessmentRepo(IContinuumDataContainer container)
        {
            _container = container;
        }

        IEnumerable<Assessment> IRepository<Assessment>.All()
        {
            return _container.Assessments.AsEnumerable(); 
        }

        public virtual void Create(Assessment item)
        {
            var openStatus = _container.Lookups.OfType<Data.AssessmentStatus>().Where(i=>i.Value == "Open").First();
            item.Status = openStatus;

            _container.Assessments.Add(item);
        }

        Assessment IRepository<Assessment>.FindById(int id)
        {
            throw new NotImplementedException();
        }

        public virtual Assessment GetCurrentAssessmentForTeam(int teamId)
        {
            return _container.Assessments.Where(i => i.TeamId == teamId).FirstOrDefault();
        }

        public virtual void SaveChanges()
        {
            _container.SaveChanges();
        }

        public void UpdateCapabilityForAssessment(Assessment assessment, IEnumerable<AssessmentItem> assessmentItems)
        {

            if (assessment.Status.Value == "Moderating" || assessment.Status.Value == "Closed")
            {
                throw new ApplicationException("You can only update capabilites for open assessments.");
            }

            foreach (var item in assessmentItems)
            {
                if (assessment.AssessmentItems.Any(i => i.CapabiltyId == item.CapabiltyId))
                {
                    var assessmentItem = assessment.AssessmentItems.First(i => i.CapabiltyId == item.CapabiltyId);
                    assessmentItem.CapabilityAchieved = item.CapabilityAchieved;
                }
                else
                {
                    assessment.AssessmentItems.Add(item);
                }
            }
        }

       
        public virtual void StartModeration(Assessment assessment)
        {
            assessment.Status = _container.Lookups.OfType<Data.AssessmentStatus>().Where(i => i.Value == "Moderating").First();
        }


        public virtual void CloseAssessment(Assessment assessment)
        {
            if (assessment.Status.Value == "Open")
            {
                throw new ApplicationException("You cannot close an assessment before it has been moderated.");
            }

            if (assessment.Status.Value == "Closed")
            {
                throw new ApplicationException("This assesment is already closed.");
            }

            assessment.Status = _container.Lookups.OfType<Data.AssessmentStatus>().Where(i => i.Value == "Closed").First();
        }


        public void CreateAssessmentForTeam(Team team)
        {
            var assessment = this.GetCurrentAssessmentForTeam(team.Id);

            if (assessment != null && (assessment.Status.Value == "Open" || assessment.Status.Value == "Moderating"))
            {
                throw new ApplicationException("You cannot a new assessment until you complete the previous one.");
            }

            assessment = new Data.Assessment();
            assessment.TeamId = team.Id;

            this.Create(assessment);
        }


        public void UpdateModerationResultForAssessment(Assessment assessment, IEnumerable<AssessmentResult> assessmentResults)
        {
            if (assessment.Status.Value == "Closed" || assessment.Status.Value == "Open")
            {
                throw new ApplicationException("You can only add Assesssment Results when the assessment is in moderation.");
            }

            foreach (var result in assessmentResults)
            {
                if(assessment.AssessmentResults.Any(i=>i.DimensionId == result.DimensionId))
                {
                    var currentResult = assessment.AssessmentResults.First(i => i.DimensionId == result.DimensionId);
                    currentResult.Rating = result.Rating;
                }
                else
                {
                    assessment.AssessmentResults.Add(result);
                }                
            } 
        }


        public IEnumerable<AssessmentItem> GetCurrentAssessmentItemsForUser(string userId)
        {
            var teamMember = _container.TeamMembers.Where(i => i.UserId == userId).FirstOrDefault();
            var assessment = GetCurrentAssessmentForTeam(teamMember.TeamId);
            return assessment.AssessmentItems.Where(i => i.TeamMemberId == teamMember.Id).AsEnumerable();
        }
    }
}
