using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Data
{
    public class GoalRepo : IRepository
    {
        private readonly Data.IContinuumDataContainer _container;

        public GoalRepo(Data.IContinuumDataContainer container)
        {
            _container = container;
        }

        public IEnumerable<Data.Goal> GetGoalsForTeam(Team team)
        {
            return GetGoalsForTeam(team.Id);
        }

        public IEnumerable<Data.Goal> GetGoalsForTeam(int teamId)
        {
            return _container.Goals.Where(i => i.TeamId == teamId);
        }

        public void CreateGoal(Goal goal)
        {
            if (goal.DueDate < DateTime.Now)
            {
                throw new ApplicationException("Goals cannot have a Due Date in the past.");
            }

            _container.Goals.Add(goal);
        }

        public void SaveChanges()
        {
            _container.SaveChanges();
        }

        public void UpdateGoal(Goal goal)
        {
            _container.Goals.Attach(goal);
            _container.SetStateForEntity(goal, EntityState.Modified);
        }

        public void DeleteGoalById(int id)
        {
            var goalToDelete = _container.Goals.Find(id);
            if (goalToDelete != null)
            {
                _container.Goals.Remove(goalToDelete);
            }
        }

        public Goal GetGoalById(int id)
        {
            return _container.Goals.Find(id);
        }
    }
}
