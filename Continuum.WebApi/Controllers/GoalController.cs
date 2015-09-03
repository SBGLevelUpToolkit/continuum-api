using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Continuum.Data;

namespace Continuum.WebApi.Controllers
{
    public class GoalController : ControllerBase
    {
        private readonly GoalRepository _goalRepository;
        private readonly ITeamRepo _teamRepo;

        
        public GoalController(Data.GoalRepository goalRepository, Data.ITeamRepo teamRepo) : base(teamRepo)
        {
            _goalRepository = goalRepository;
            _teamRepo = teamRepo; 
        }

        public IEnumerable<Models.Goal> Get()
        {
            var team = GetTeamForUser();
            return _goalRepository.GetActiveGoalsForTeam(team).Select(i => new Models.Goal() { CapabilityId = i.CapabiltyId, DueDate =i.DueDate, Notes = i.Title });
        }

        public void Post(Models.Goal goal)
        {
            try
            {
                _goalRepository.CreateGoal(new Data.Goal() { CapabiltyId = goal.CapabilityId, DueDate = goal.DueDate, Description = goal.Notes });
                _goalRepository.SaveChanges();
            }
            catch (ApplicationException ex)
            {
                throw ExceptionBuilder.FromException(ex);
            }
        }

        public void Put(int id, Models.Goal goal)
        {
            _goalRepository.UpdateGoal(new Data.Goal() { Id = id, CapabiltyId = goal.CapabilityId, DueDate = goal.DueDate, Description = goal.Notes, Completed = goal.Completed });
            _goalRepository.SaveChanges();
        }

        public void Delete(int id)
        {
            _goalRepository.DeleteGoalById(id);
            _goalRepository.SaveChanges(); 
        }
    }
}
