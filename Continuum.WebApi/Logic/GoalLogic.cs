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
            return _goalRepository.GetGoalsForTeam(team.Id)
                .Select(i => new Models.Goal() 
                { 
                    Id = i.Id, 
                    DimensionId = i.Capabilty.DimensionId, 
                    DimensionText = i.Capabilty.Dimension.Name,
                    CapabilityId = i.CapabiltyId, 
                    CapabilityText = i.Capabilty.Description,
                    DueDate = i.DueDate, 
                    Notes = i.Description 
                });
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
            var team = _teamLogic.GetTeamForUser();

            var currentGoal = _goalRepository.GetGoalById(id);
            if(currentGoal != null)
            {
                currentGoal.Completed = goal.Completed;
                currentGoal.Description = goal.Notes;
                currentGoal.CapabiltyId = goal.CapabilityId;
                currentGoal.DueDate = goal.DueDate;

                _goalRepository.UpdateGoal(currentGoal);
                _goalRepository.SaveChanges();

                string message = String.Format("Updated Goal {0} Status: {1}", currentGoal.Id, currentGoal.Completed);
                Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error() { Message = message });
            }
            else
            {
                throw new ApplicationException("Invalid goal Id.");
            }
        }

        internal void DeleteGoal(int id)
        {
            _goalRepository.DeleteGoalById(id);
            _goalRepository.SaveChanges(); 
        }
    }
}