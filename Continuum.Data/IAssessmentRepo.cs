using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Data
{
    public interface IAssessmentRepo : IRepository<Data.Assessment>
    {
        Data.Assessment GetCurrentAssessmentForTeam(int teamId);

        void UpdateCapabilityForAssessment(Assessment assessment, IEnumerable<AssessmentItem> assessmentItems);

        void UpdateModerationResultForAssessment(Assessment assessment, IEnumerable<Data.AssessmentResult> assessmentResults);

        void StartModeration(Assessment assessment);

        void CloseAssessment(Assessment assessment);

        void CreateAssessmentForTeam(Team team);

        IEnumerable<Data.AssessmentItem> GetCurrentAssessmentItemsForUser(string userId);

    }
}
