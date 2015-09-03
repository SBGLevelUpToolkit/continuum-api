using System;
using Continuum.WebApi.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Web.Http;

namespace Continuum.Tests
{
    [TestClass]
    public class GoalTests
    {

        Data.IContinuumDataContainer _mockContainer;
        Data.TeamRepository _teamRepo;
        Data.GoalRepository _goalRepo;


        [TestInitialize]
        public void Setup() 
        {
            _mockContainer = new Data.Mocks.MockContainer();
            _teamRepo = new Data.TeamRepository(_mockContainer);
            _goalRepo = new Data.GoalRepository(_mockContainer);
        }

        [TestMethod]
        public void GetGoalsMustReturnOnlyActiveGoals()
        {
            Data.Team team;
            Data.TeamMember teamMember;
            CreateTeamAndTeamMember(out team, out teamMember);

            _mockContainer.Goals.Add(new Data.Goal() { Completed = true, TeamId = team.Id });
            _mockContainer.Goals.Add(new Data.Goal() { Completed = false, TeamId = team.Id });

            var controller = new GoalController(_goalRepo, _teamRepo);

            var result = controller.Get();

            Assert.IsTrue(result.Count() == 1);
        }

        [TestMethod]
        public void TestThatCreateGoalCreates()
        {
            var controller = new GoalController(_goalRepo, _teamRepo);

            WebApi.Models.Goal goal = new WebApi.Models.Goal()
            {
                CapabilityId = 1,
                DueDate = DateTime.Now.AddDays(1),
                Notes = "Some Notes"
            };

            controller.Post(goal);

            Assert.IsTrue(_mockContainer.Goals.Count() == 1);
        }

        [TestMethod]
        public void TestThatCreateGoalInPastThrowsException()
        {
            var controller = new GoalController(_goalRepo, _teamRepo);

            WebApi.Models.Goal goal = new WebApi.Models.Goal()
            {
                CapabilityId = 1,
                DueDate = DateTime.Now.Subtract(TimeSpan.FromDays(2)),
                Notes = "Some Notes"
            };

            try
            {
                controller.Post(goal);
                Assert.Fail();
            }
            catch(HttpResponseException ex)
            {
                Assert.IsTrue(ex.Response.StatusCode == System.Net.HttpStatusCode.InternalServerError);
            }
        }

        [TestMethod]
        public void TestThatUpdateGoalUpdates()
        {
            Data.Team team;
            Data.TeamMember teamMember;
            CreateTeamAndTeamMember(out team, out teamMember);

            _mockContainer.Goals.Add(new Data.Goal() { Completed = false, TeamId = team.Id, Id = 1 });

            var controller = new GoalController(_goalRepo, _teamRepo);

            WebApi.Models.Goal goal = new WebApi.Models.Goal()
            {
                CapabilityId = 1,
                DueDate = DateTime.Now.AddDays(1),
                Notes = "Some Notes",
                Completed = true 
            };

            controller.Put(1,goal);

            var updatedGoal = _mockContainer.Goals.Where(i => i.Id == 1).FirstOrDefault();
            Assert.IsNotNull(updatedGoal);
            Assert.IsTrue(updatedGoal.Completed == true);
            Assert.IsTrue(updatedGoal.DueDate == goal.DueDate);

        }

        [TestMethod]
        public void TestThatDeleteDeletes()
        {
            Data.Team team;
            Data.TeamMember teamMember;
            CreateTeamAndTeamMember(out team, out teamMember);

            _mockContainer.Goals.Add(new Data.Goal() { Completed = false, TeamId = team.Id, Id = 1 });

            var controller = new GoalController(_goalRepo, _teamRepo);

            controller.Delete(1);

            Assert.IsTrue(_mockContainer.Goals.Count() == 0);
        }

        private void CreateTeamAndTeamMember(out Data.Team team, out Data.TeamMember teamMember)
        {
            team = new Data.Team() { Id = 1 };
            teamMember = new Data.TeamMember() { UserId = TestUser, Id = 1 };
            teamMember.Team = team;
            teamMember.TeamId = team.Id;
            team.TeamMembers.Add(teamMember);
            _mockContainer.Teams.Add(team);
            _mockContainer.TeamMembers.Add(teamMember);
        }

        public string TestUser
        {
            get
            {
                string user = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                return user;
            }
        }
    }
}
