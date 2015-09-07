using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Continuum.Data;

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

        public Models.Assessment Get()
        {
            try
            {
                return _assessmentLogic.GetAssessment();
            }
            catch (ApplicationException ex)
            {
                throw ExceptionBuilder.FromException(ex);
            }            
        }

        public void Put(IEnumerable<Models.AssessmentResult> assessmentResults)
        {
            try
            {
                _assessmentLogic.UpdateAssessmentResults(assessmentResults);
            }
            catch (ApplicationException ex)
            {
                throw ExceptionBuilder.FromException(ex);
            }
        }

        public void Post(IEnumerable<Models.AssessmentItem> assessmentItems)
        {
            try
            {
                _assessmentLogic.UpdateAssessmentItems(assessmentItems);
            }
            catch (ApplicationException ex)
            {
                throw ExceptionBuilder.FromException(ex);
            } 
        }

        [Route("api/assessment/create")]
        [Filters.TeamAdminFilter]
        public void Create()
        {
            try
            {
                _assessmentLogic.CreateAssessment();
            }
            catch (ApplicationException ex)
            {
                throw ExceptionBuilder.FromException(ex);
            }   
        }

        [Route("api/assessment/moderate")]
        [Filters.TeamAdminFilter]
        public void Moderate()
        {
            try
            {
                _assessmentLogic.ModerateAssessment();
            }
            catch (ApplicationException ex)
            {
                throw ExceptionBuilder.FromException(ex);
            } 
        }

        [Route("api/assessment/close")]
        [Filters.TeamAdminFilter]
        public void Close()
        {
            try
            {
                _assessmentLogic.CloseAssessment();
            }
            catch (ApplicationException ex)
            {
                throw ExceptionBuilder.FromException(ex);
            } 
        }
    }
}
