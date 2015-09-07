using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace Continuum.WebApi.Logic
{
    public class GoalLogic : LogicBase
    {
        private readonly Data.GoalRepo _goalRepository;
        private readonly Logic.TeamLogic _teamLogic;
        private readonly Data.DimensionRepo _dimensionRepo;

        public GoalLogic(Data.GoalRepo goalRepository, Data.DimensionRepo dimensionRepo, Logic.TeamLogic teamLogic, System.Security.Principal.IPrincipal principal)
            : base(principal)
        {
            _goalRepository = goalRepository;
            _teamLogic = teamLogic;
            _dimensionRepo = dimensionRepo;
        }

        internal IEnumerable<Models.Goal> ListGoalsForTeam(Models.Team team)
        {
            return _goalRepository.GetActiveGoalsForTeam(team.Id)
                .Select(i => new Models.Goal() { Id = i.Id, CapabilityId = i.CapabiltyId, DueDate = i.DueDate, Notes = i.Description });
        }

        internal void CreateGoal(Models.Goal goal)
        {
            var team = _teamLogic.GetTeamForUser();

            var capability = _dimensionRepo.FindCapabilityById(goal.CapabilityId);
            if (capability == null)
            {
                throw new ApplicationException("Invalid capability Id.");
            }

            var newGoal = new Data.Goal()
            {
                Capabilty = capability,
                DueDate = goal.DueDate,
                Title = "New Goal",
                Description = goal.Notes,
                TeamId = team.Id
            };

            _goalRepository.CreateGoal(newGoal);
            _goalRepository.SaveChanges();
        }

        internal void UpdateGoalById(int id, Models.Goal goal)
        {
            _goalRepository.UpdateGoal(new Data.Goal() { Id = id, CapabiltyId = goal.CapabilityId, DueDate = goal.DueDate, Title= "Goal", Description = goal.Notes, Completed = goal.Completed });
            _goalRepository.SaveChanges();
        }

        internal void DeleteGoal(int id)
        {
            _goalRepository.DeleteGoalById(id);
            _goalRepository.SaveChanges(); 
        }
    }
}