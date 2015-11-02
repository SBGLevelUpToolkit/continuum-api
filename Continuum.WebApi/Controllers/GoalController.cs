using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Continuum.Core.Models;
using Continuum.WebApi.Filters;

namespace Continuum.WebApi.Controllers
{
    public class GoalController : ControllerBase
    {
        private readonly Continuum.Data.GoalRepo _goalRepository;
        private readonly Continuum.Data.TeamRepo _teamRepo;
        private readonly Continuum.Data.DimensionRepo _dimensionRepo;

        public GoalController(Data.GoalRepo goalRepository, Data.TeamRepo teamRepo, Data.DimensionRepo dimensionRepo)
        {
            _goalRepository = goalRepository;
            _teamRepo = teamRepo;
            _dimensionRepo = dimensionRepo;
        }

        private Logic.TeamLogic TeamLogic
        {
            get
            {
                var user = CurrentUser == null ? this.User : CurrentUser;
                return new Logic.TeamLogic(_teamRepo, user);
            }
        }

        private Logic.GoalLogic GoalLogic
        {
            get
            {
                var user = CurrentUser == null ? this.User : CurrentUser;
                var teamLogic = new Logic.TeamLogic(_teamRepo, user);
                return new Logic.GoalLogic(_goalRepository, _dimensionRepo, teamLogic, user);
            }
        }

        public IEnumerable<Goal> Get()
        {
            var team = TeamLogic.GetTeamForUser();
            return GoalLogic.ListGoalsForTeam(team).ToList(); 
        }

        /// <summary>
        /// Creates a new Goal.
        /// </summary>
        /// <param name="goal"></param>
        /// <returns></returns>
        [ApplicationExceptionFilter]
        [ValidationResultFilter]
        [ResponseType(typeof(Goal))]
        public IHttpActionResult Post(Goal goal)
        {
            if (ModelState.IsValid)
            {
                var created = GoalLogic.CreateGoal(goal);

                string uri = Url.Link("Default", new { id = created.Id });

                return Created(uri, created);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [ValidationResultFilter]
        public IHttpActionResult Put(int id, Goal goal)
        {
            if (GoalLogic.GoalExists(id))
            {
                GoalLogic.UpdateGoalById(id, goal);
                return Content(HttpStatusCode.OK, GoalLogic.GetGoal(id));
            }
            else
            {
                return NotFound();
            }
        }

        public IHttpActionResult Delete(int id)
        {
            if (GoalLogic.GoalExists(id))
            {
                GoalLogic.DeleteGoal(id);
                return Ok(); 
            }
            else
            {
                return NotFound();
            }
        }
    }
}
