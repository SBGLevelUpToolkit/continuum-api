using System;
using Continuum.WebApi.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Web.Http;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;

namespace Continuum.Tests
{
    [TestClass]
    public class GoalTests
    {
        HttpRequestMessage _request;

        Data.IContinuumDataContainer _mockContainer;
        Data.TeamRepo _teamRepo;
        Data.GoalRepo _goalRepo;
        Data.DimensionRepo _dimensionRepo;
        GoalController _controller;

        [TestInitialize]
        public void Setup() 
        {
            var config = new HttpConfiguration();

            config.Routes.MapHttpRoute(
                name: "Default",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            _request = new HttpRequestMessage(HttpMethod.Get, "http://localhost");
            _request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            _request.Properties[HttpPropertyKeys.HttpRouteDataKey] = new HttpRouteData(new HttpRoute());

            _mockContainer = new Data.Mocks.MockContainer();
            _teamRepo = new Data.TeamRepo(_mockContainer);
            _goalRepo = new Data.GoalRepo(_mockContainer);
            _dimensionRepo = new Data.DimensionRepo(_mockContainer);
            

            var identity = new System.Security.Principal.GenericIdentity("alice@example.com");
            var princpal = new System.Security.Principal.GenericPrincipal(identity, new string[] { });

            _controller = new GoalController(_goalRepo, _teamRepo, _dimensionRepo);
            _controller.User = princpal;
            _controller.Request = _request;
        }

        /*
         * Replaced by runscope. These tests need to be converted to work against the Logic layer. 
         
        [TestMethod]
        public void TestThatCreateGoalCreates()
        {
            _mockContainer.Capabilities.Add(new Data.Capability() { Id = 1, Dimension = new Data.Dimension() });

            Core.Models.Goal goal = new Core.Models.Goal()
            {
                CapabilityId = 1,
                DueDate = DateTime.Now.AddDays(1),
                Notes = "Some Notes"
            };

            _controller.Post(goal);

            Assert.IsTrue(_mockContainer.Goals.Count() == 1);
        }*/

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestThatCreateGoalInPastThrowsException()
        {
            _mockContainer.Capabilities.Add(new Data.Capability() { Id = 1, Dimension = new Data.Dimension() { Id = 1, Name = "Bob" } });

            Core.Models.Goal goal = new Core.Models.Goal()
            {
                CapabilityId = 1,
                DueDate = DateTime.Now.Subtract(TimeSpan.FromDays(2)),
                Notes = "Some Notes"
            };

            _controller.Post(goal);
            Assert.Fail();
        }
        [Ignore]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestThatCreateGoalNoNotesWorks()
        {
            _mockContainer.Capabilities.Add(new Data.Capability() { Id = 1, Dimension = new Data.Dimension() { Id = 1, Name = "Bob" } });

            Core.Models.Goal goal = new Core.Models.Goal()
            {
                CapabilityId = 1,
                DueDate = DateTime.Now.AddDays(2)
            };

            _controller.Post(goal);
            Assert.IsTrue(goal.Id>0);
        }

        //[TestMethod]
        public void TestThatUpdateGoalUpdates()
        {
            Data.Team team;
            Data.TeamMember teamMember;
            CreateTeamAndTeamMember(out team, out teamMember);

            _mockContainer.Goals.Add(new Data.Goal() { Completed = false, TeamId = team.Id, Id = 1 });

            Core.Models.Goal goal = new Core.Models.Goal()
            {
                CapabilityId = 1,
                DueDate = DateTime.Now.AddDays(1),
                Notes = "Some Notes",
                Completed = true 
            };

            _controller.Put(1,goal);

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

            _controller.Delete(1);

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
