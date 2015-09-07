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
        private readonly DimensionRepo _dimensionRepo;

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

        public IEnumerable<Models.Goal> Get()
        {
            var team = TeamLogic.GetTeamForUser();
            return GoalLogic.ListGoalsForTeam(team).ToList(); 
        }

        [ApplicationExceptionFilter]
        [ValidationResultFilter]
        public void Post(Models.Goal goal)
        {
            GoalLogic.CreateGoal(goal);
        }

        [ValidationResultFilter]
        public void Put(int id, Models.Goal goal)
        {
            GoalLogic.UpdateGoalById(id, goal);       
        }

        public void Delete(int id)
        {
            GoalLogic.DeleteGoal(id); 
        }
    }
}
