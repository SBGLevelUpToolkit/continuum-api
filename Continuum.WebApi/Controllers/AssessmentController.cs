using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Continuum.Core.Models;
using Continuum.WebApi.Filters; 

namespace Continuum.WebApi.Controllers
{
    [Authorize]
    public class AssessmentController : ControllerBase
    {
        private readonly Data.AssessmentRepo _assessmentRepo;
        private readonly Data.TeamRepo _teamRepo;
        private readonly Data.DimensionRepo _dimensionRepo; 
  
        public AssessmentController(Data.AssessmentRepo assessmentRepo, Data.TeamRepo teamRepo, Data.DimensionRepo dimensionRepo)
        {
            _assessmentRepo = assessmentRepo;
            _teamRepo = teamRepo;
            _dimensionRepo = dimensionRepo; 
        }

        private Logic.AssessmentLogic AssessmentLogic
        {
            get
            {
                return new Logic.AssessmentLogic(_assessmentRepo, _teamRepo, _dimensionRepo , CurrentUser == null ? this.User : CurrentUser);
            }
        }

        /// <summary>
        /// Returns the current active Assessment for the current user's team.
        /// </summary>
        /// <remarks>Current Open or Moderating Assessment. Will return 204 if no Assessment is available.</remarks>
        [ApplicationExceptionFilter]
        public IHttpActionResult Get()
        {
            if (AssessmentLogic.AssessmentIsAvailable())
            {
                return Content<Core.Models.Assessment>(HttpStatusCode.OK, AssessmentLogic.GetAssessment());
            }
            else
            {
                return ResponseMessage( new HttpResponseMessage(HttpStatusCode.NoContent));
            }
        }

        /// <summary>
        /// Updates the moderated assessment results for the current assessment. Note only the Team Admin can call this operation.
        /// </summary>
        /// <param name="assessmentResults"></param>
        [ApplicationExceptionFilter]
        public IHttpActionResult Put(IEnumerable<AssessmentResult> assessmentResults)
        {
            if (ModelState.IsValid)
            {
                AssessmentLogic.UpdateAssessmentResults(assessmentResults);
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Updates the assessment results for the current user on the users current assessment.
        /// </summary>
        /// <param name="assessmentItems">A collection of <paramref name="Models.AssessmentItem"/></param>
        [ApplicationExceptionFilter]
        public void Post(IEnumerable<AssessmentItem> assessmentItems)
        {
            AssessmentLogic.UpdateAssessmentItems(assessmentItems);
        }

        /// <summary>
        /// Creates a new assesssment for the current user's team.
        /// </summary>
        /// <exception cref="ApplicationException">Throws InternalServerError (500) if a current assessment already exists.</exception>
        [Route("api/assessment/create")]
        [Filters.TeamAdminFilter]
        [ApplicationExceptionFilter]
        public void Create()
        {
            AssessmentLogic.CreateAssessment();
        }

        /// <summary>
        /// Puts the current assessment into Moderation. Note that only the Team Admin can call this operation.
        /// </summary>
        /// <exception cref="ApplicationException">Throws InternalServerError (500) if there is no current assessment or if the current assessment is not in the 'Open' status.</exception>
        [Route("api/assessment/moderate")]
        [Filters.TeamAdminFilter]
        [ApplicationExceptionFilter]
        public void Moderate()
        {
            AssessmentLogic.ModerateAssessment();
        }

        /// <summary>
        /// Closes the current assessment. Note that onlt the Team Admin can call this operation.
        /// </summary>
        /// <exception cref="ApplicationException">Throws InternalServerError (500) if there is no current assessment or if the current assessment us not in the 'Moderating' status.</exception>
        [Route("api/assessment/close")]
        [Filters.TeamAdminFilter]
        [ApplicationExceptionFilter]
        public void Close()
        {
            AssessmentLogic.CloseAssessment();
        }

        /// <summary>
        /// Reopens an assessment in Moderating status. Note that onlt the Team Admin can call this operation.
        /// </summary>
        /// <exception cref="ApplicationException">Throws InternalServerError (500) if there is no current assessment or if the current assessment us not in the 'Moderating' status.</exception>
        [Route("api/assessment/reopen")]
        [Filters.TeamAdminFilter]
        [ApplicationExceptionFilter]
        public void Reopen()
        {
            AssessmentLogic.ReopenAssessment();
        }

         [Route("api/assessment/score")]
         [ApplicationExceptionFilter]
        public Core.Models.AssessmentScoringResult GetScore()
        {
            return AssessmentLogic.ScoreCurrentAssessment(); 
        }
    }
}
