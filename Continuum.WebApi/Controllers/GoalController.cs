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
    public class GoalController : ControllerBase
    {
        private readonly GoalRepo _goalRepository;
        private readonly TeamRepo _teamRepo;
        private readonly Logic.GoalLogic _goalLogic;
        private readonly Logic.TeamLogic _teamLogic;

        public GoalController(Data.GoalRepo goalRepository, Data.TeamRepo teamRepo)
        {
            _goalRepository = goalRepository;
            _teamRepo = teamRepo;

            var user = CurrentUser == null ? this.User : CurrentUser;
            _teamLogic = new Logic.TeamLogic(_teamRepo, user);
            _goalLogic = new Logic.GoalLogic(_goalRepository, user);
        }

        public IEnumerable<Models.Goal> Get()
        {
            var team = _teamLogic.GetTeamForUser();
            return _goalLogic.ListGoalsForTeam(team); 
        }

        [ApplicationExceptionFilter]
        public void Post(Models.Goal goal)
        {
            _goalLogic.CreateGoal(goal);
        }

        public void Put(int id, Models.Goal goal)
        {
            _goalLogic.UpdateGoalById(id, goal);       
        }

        public void Delete(int id)
        {
            _goalLogic.DeleteGoal(id); 
        }
    }
}
