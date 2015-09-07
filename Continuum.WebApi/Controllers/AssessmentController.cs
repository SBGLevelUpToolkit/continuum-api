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
        private readonly Data.IAssessmentRepo _assessmentRepo;
        private readonly Data.ITeamRepo _teamRepo;
        private readonly Logic.AssessmentLogic _assessmentLogic; 

        public AssessmentController(Data.IAssessmentRepo assessmentRepo, Data.ITeamRepo teamRepo) : base(teamRepo)
        {
            _assessmentRepo = assessmentRepo;
            _teamRepo = teamRepo;
            _assessmentLogic = new Logic.AssessmentLogic(_assessmentRepo, _teamRepo, CurrentUser == null ?  this.User : CurrentUser);
        }

        [ApplicationExceptionFilter]
        public Models.Assessment Get()
        {
            return _assessmentLogic.GetAssessment();
        }

        [ApplicationExceptionFilter]
        public void Put(IEnumerable<Models.AssessmentResult> assessmentResults)
        {
            _assessmentLogic.UpdateAssessmentResults(assessmentResults);
        }

        [ApplicationExceptionFilter]
        public void Post(IEnumerable<Models.AssessmentItem> assessmentItems)
        {
            _assessmentLogic.UpdateAssessmentItems(assessmentItems);
        }

        [Route("api/assessment/create")]
        [Filters.TeamAdminFilter]
        [ApplicationExceptionFilter]
        public void Create()
        {
            _assessmentLogic.CreateAssessment();
        }

        [Route("api/assessment/moderate")]
        [Filters.TeamAdminFilter]
        [ApplicationExceptionFilter]
        public void Moderate()
        {
            _assessmentLogic.ModerateAssessment();
        }

        [Route("api/assessment/close")]
        [Filters.TeamAdminFilter]
        [ApplicationExceptionFilter]
        public void Close()
        {
            _assessmentLogic.CloseAssessment();
        }
    }
}
