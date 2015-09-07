using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Continuum.Data;
using Continuum.WebApi.Filters; 

namespace Continuum.WebApi.Controllers
{
    [Authorize]
    public class AssessmentController : ControllerBase
    {
        private readonly Data.AssessmentRepo _assessmentRepo;
        private readonly Data.TeamRepo _teamRepo;
  
        public AssessmentController(Data.AssessmentRepo assessmentRepo, Data.TeamRepo teamRepo)
        {
            _assessmentRepo = assessmentRepo;
            _teamRepo = teamRepo;
        }

        private Logic.AssessmentLogic AssessmentLogic
        {
            get
            {
                return new Logic.AssessmentLogic(_assessmentRepo, _teamRepo, CurrentUser == null ? this.User : CurrentUser);
            }
        }

        [ApplicationExceptionFilter]
        public Models.Assessment Get()
        {
            return AssessmentLogic.GetAssessment();
        }

        [ApplicationExceptionFilter]
        public void Put(IEnumerable<Models.AssessmentResult> assessmentResults)
        {
            AssessmentLogic.UpdateAssessmentResults(assessmentResults);
        }

        [ApplicationExceptionFilter]
        public void Post(IEnumerable<Models.AssessmentItem> assessmentItems)
        {
            AssessmentLogic.UpdateAssessmentItems(assessmentItems);
        }

        [Route("api/assessment/create")]
        [Filters.TeamAdminFilter]
        [ApplicationExceptionFilter]
        public void Create()
        {
            AssessmentLogic.CreateAssessment();
        }

        [Route("api/assessment/moderate")]
        [Filters.TeamAdminFilter]
        [ApplicationExceptionFilter]
        public void Moderate()
        {
            AssessmentLogic.ModerateAssessment();
        }

        [Route("api/assessment/close")]
        [Filters.TeamAdminFilter]
        [ApplicationExceptionFilter]
        public void Close()
        {
            AssessmentLogic.CloseAssessment();
        }
    }
}
