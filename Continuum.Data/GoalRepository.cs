using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Data
{
    public class GoalRepository
    {
        private readonly Data.IContinuumDataContainer _container;

        public GoalRepository(Data.IContinuumDataContainer container)
        {
            _container = container;
        }

        public IEnumerable<Data.Goal> GetActiveGoalsForTeam(Team team)
        {
            return _container.Goals.Where(i => i.TeamId == team.Id && i.Completed == false);
        }

        public IEnumerable<Data.Goal> GetActiveGoalsForTeam(int teamId)
        {
            return _container.Goals.Where(i => i.TeamId == teamId && i.Completed == false);
        }

        public void CreateGoal(Goal goal)
        {
            if (goal.DueDate < DateTime.Now)
            {
                throw new ApplicationException("Goals cannot have a Due Date in the past.");
            }

            _container.Goals.Add(goal);
        }

        public int SaveChanges()
        {
            return _container.SaveChanges();
        }

        public void UpdateGoal(Goal goal)
        {
            var goalToUpdate = _container.Goals.Find(goal.Id);
            if (goalToUpdate == null)
            {
                throw new ApplicationException(string.Format("No goal with id {0}"));
            }

            goalToUpdate.CapabiltyId = goal.CapabiltyId;
            goalToUpdate.Completed = goal.Completed;
            goalToUpdate.Description = goal.Description;
            goalToUpdate.DueDate = goal.DueDate;
           
        }

        public void DeleteGoalById(int id)
        {
            var goalToDelete = _container.Goals.Find(id);
            if (goalToDelete != null)
            {
                _container.Goals.Remove(goalToDelete);
            }
        }
    }
}
