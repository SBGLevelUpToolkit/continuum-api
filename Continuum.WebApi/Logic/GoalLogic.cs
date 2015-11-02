using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using Continuum.Core.Models;

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

        internal IEnumerable<Goal> ListGoalsForTeam(Team team)
        {
            return _goalRepository.GetGoalsForTeam(team.Id)
                .Select(i => new Goal() 
                { 
                    Id = i.Id, 
                    DimensionId = i.Capabilty.DimensionId, 
                    DimensionText = i.Capabilty.Dimension.Name,
                    CapabilityId = i.CapabiltyId, 
                    CapabilityText = i.Capabilty.Description,
                    DueDate = i.DueDate, 
                    Notes = i.Description,
                    Completed = i.Completed
                });
        }

        internal Goal CreateGoal(Goal goal)
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

            return new Goal()
                {
                    Id = newGoal.Id,
                    DimensionId = newGoal.Capabilty.DimensionId,
                    DimensionText = newGoal.Capabilty.Dimension.Name,
                    CapabilityId = newGoal.CapabiltyId,
                    CapabilityText = newGoal.Capabilty.Description,
                    DueDate = newGoal.DueDate,
                    Notes = newGoal.Description,
                    Completed = newGoal.Completed
                };
        }

        internal void UpdateGoalById(int id, Goal goal)
        {
          
            var currentGoal = _goalRepository.GetGoalById(id);
            if(currentGoal != null)
            {
                currentGoal.Completed = goal.Completed;
                currentGoal.Description = goal.Notes;
                currentGoal.CapabiltyId = goal.CapabilityId;
                currentGoal.DueDate = goal.DueDate;
               
                _goalRepository.SaveChanges();
            }
            else
            {
                throw new ApplicationException("Invalid goal Id.");
            }
        }

        public bool GoalExists(int id)
        {
            return _goalRepository.GoalExists(id);
        }

        internal void DeleteGoal(int id)
        {
            _goalRepository.DeleteGoalById(id);
            _goalRepository.SaveChanges(); 
        }

        internal Core.Models.Goal GetGoal(int id)
        {
            var goal = _goalRepository.GetGoalById(id);

            return new Goal()
                {
                    Id = goal.Id,
                    DimensionId = goal.Capabilty.DimensionId,
                    DimensionText = goal.Capabilty.Dimension.Name,
                    CapabilityId = goal.CapabiltyId,
                    CapabilityText = goal.Capabilty.Description,
                    DueDate = goal.DueDate,
                    Notes = goal.Description,
                    Completed = goal.Completed
                };
        }
    }
}