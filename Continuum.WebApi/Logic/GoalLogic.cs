using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Continuum.WebApi.Logic
{
    public class GoalLogic : LogicBase
    {
        private readonly Data.GoalRepo _goalRepository;

        public GoalLogic(Data.GoalRepo goalRepository, System.Security.Principal.IPrincipal principal)
            : base(principal)
        {
            _goalRepository = goalRepository;
        }

        internal IEnumerable<Models.Goal> ListGoalsForTeam(Models.Team team)
        {
            return _goalRepository.GetActiveGoalsForTeam(team.Id)
                .Select(i => new Models.Goal() { CapabilityId = i.CapabiltyId, DueDate = i.DueDate, Notes = i.Title });
        }

        internal void CreateGoal(Models.Goal goal)
        {
            _goalRepository.CreateGoal(new Data.Goal() { CapabiltyId = goal.CapabilityId, DueDate = goal.DueDate, Description = goal.Notes });
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