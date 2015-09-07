using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Continuum.WebApi.Logic
{
    public class GoalLogic : LogicBase
    {
        private readonly Data.GoalRepo _goalRepository;
        private readonly Logic.TeamLogic _teamLogic; 

        public GoalLogic(Data.GoalRepo goalRepository, Logic.TeamLogic teamLogic, System.Security.Principal.IPrincipal principal)
            : base(principal)
        {
            _goalRepository = goalRepository;
            _teamLogic = teamLogic;
        }

        internal IEnumerable<Models.Goal> ListGoalsForTeam(Models.Team team)
        {
            return _goalRepository.GetActiveGoalsForTeam(team.Id)
                .Select(i => new Models.Goal() { CapabilityId = i.CapabiltyId, DueDate = i.DueDate, Notes = i.Title });
        }

        internal void CreateGoal(Models.Goal goal)
        {
            var team = _teamLogic.GetTeamForUser();

            _goalRepository.CreateGoal(new Data.Goal() { CapabiltyId = goal.CapabilityId, DueDate = goal.DueDate, Description = goal.Notes, TeamId = team.Id });
            _goalRepository.SaveChanges();
        }

        internal void UpdateGoalById(int id, Models.Goal goal)
        {
            _goalRepository.UpdateGoal(new Data.Goal() { Id = id, CapabiltyId = goal.CapabilityId, DueDate = goal.DueDate, Description = goal.Notes, Completed = goal.Completed });
            _goalRepository.SaveChanges();
        }

        internal void DeleteGoal(int id)
        {
            _goalRepository.DeleteGoalById(id);
            _goalRepository.SaveChanges(); 
        }
    }
}