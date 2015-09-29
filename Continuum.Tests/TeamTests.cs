using System;
using Continuum.WebApi.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Web.Http;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using System.Collections.Generic;
using System.Security;

namespace Continuum.Tests
{
    [TestClass]
    public class TeamTests
    {
        HttpRequestMessage _request;

        Data.Mocks.MockContainer _mockContainer;
        Data.TeamRepo _teamRepository;
        Data.DimensionRepo _dimensionRepo;
        Data.AssessmentRepo _assessmentRepo;
        
        [TestInitialize]
        public void SetUp()
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
            _teamRepository = new Data.TeamRepo(_mockContainer);
            _dimensionRepo = new Data.DimensionRepo(_mockContainer);
            _assessmentRepo = new Data.AssessmentRepo(_mockContainer);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestThatCreatingADuplicateTeamThrowsException()
        {
        
            _mockContainer.Teams.Add(new Data.Team() { Name = "Test Team"});

            TeamController teamController = new TeamController(_teamRepository, _assessmentRepo, _dimensionRepo);
        
            Core.Models.Team newTeam = new Core.Models.Team() 
            {
                Name = _mockContainer.Teams.First().Name
            };

                teamController.Post(newTeam);
                Assert.Fail("Must not create duplicate teams");
        }

        [TestMethod]
        public void TestThatCreatingANewTeamIsAllowed()
        {
            TeamController teamController = new TeamController(_teamRepository, _assessmentRepo, _dimensionRepo);
            teamController.Request = _request;

            Core.Models.Team newTeam = new Core.Models.Team()
            {
                Name = Guid.NewGuid().ToString()
            };

            IHttpActionResult result = teamController.Post(newTeam);

            //Assert.IsTrue(result.StatusCode == System.Net.HttpStatusCode.Created);
            Assert.IsTrue(_mockContainer.Teams.Where(i => i.Name == newTeam.Name).Count() == 1);
        }

        [TestMethod]
        public void TestThatUserWhoCreatesTeamIsSetAsTeamAdmin()
        {
           /// var user = System.Security.Principal.WindowsIdentity.GetCurrent();

           var identity = new System.Security.Principal.GenericIdentity("TestUser");
           var princpal = new System.Security.Principal.GenericPrincipal(identity, new string[] { });

           TeamController.CurrentUser = princpal;
           TeamController teamController = CreateTeamController();
           teamController.Request = _request;
           teamController.User = princpal;
           
            Core.Models.Team newTeam = new Core.Models.Team() 
            {
                Name = Guid.NewGuid().ToString()
            };

            var result = teamController.Post(newTeam);

            var team = _mockContainer.Teams.Where(i => i.Name == newTeam.Name).FirstOrDefault();
            Assert.IsNotNull(team, "Could not find new team");

            var teamMember = team.TeamMembers.Where(i => i.UserId == identity.Name).FirstOrDefault();

            string allUsers = String.Join(",",team.TeamMembers.Select(i => i.UserId + i.IsAdmin.ToString()).ToArray());


            Assert.IsNotNull(teamMember, "User was not assigned to team. The current user is " + identity.Name + " Current Users:" + allUsers);

            Assert.IsTrue(teamMember.IsAdmin, "User was not created as administrator.");

        }

        private TeamController CreateTeamController()
        {
            TeamController teamController = new TeamController(_teamRepository, _assessmentRepo, _dimensionRepo);
            return teamController;
        }

        [TestMethod]
        public void TestThatTeamCreatedWithDefaultAvatar()
        {
            
        }

        [TestMethod]
        public void TestThatAddedNewUserIsAddedAndNotAdmin()
        {
            
        }

        [TestMethod]
        public void TestThatTeamWithNoAssessmentsHasDefaultRating()
        {
            CreateTeamWithMember();

           var identity = new System.Security.Principal.GenericIdentity("TestUser");
           var principal = new System.Security.Principal.GenericPrincipal(identity, new string[] { });

           Continuum.WebApi.Logic.AssessmentLogic assessmentLogic = new WebApi.Logic.AssessmentLogic(_assessmentRepo, _teamRepository, _dimensionRepo, principal);

           int rating = assessmentLogic.GetCurrentLevelForTeam();

           Assert.IsTrue(rating == 1, "Rating must be 1 if there is not assessment info.");
            
        }

        private void CreateTeamWithMember()
        {
            var teamMember = new Data.TeamMember() { UserId = "TestUser" };
            var team = new Data.Team() { Id = 0 };
            team.TeamMembers.Add(teamMember);
            teamMember.Team = team;
            _mockContainer.Teams.Add(team);
            _mockContainer.TeamMembers.Add(teamMember);
        }

        [TestMethod]
        public void TestThatTeamRatingIsAverageOfLatestAssessment()
        {
            CreateTeamWithMember();

            CreateAssessmentWithRetults(3, DateTime.Now, "Closed");

            Continuum.WebApi.Logic.AssessmentLogic assessmentLogic = CreateAssessmentLogic();

           int rating = assessmentLogic.GetCurrentLevelForTeam();

           Assert.IsTrue(rating == 3, "Rating should be average of latest assessment.");     
        }

        [TestMethod]
        public void TestThatTeamRatingBasedOnLatestAssessment()
        {
         CreateTeamWithMember();

            CreateAssessmentWithRetults(5, DateTime.Now.Subtract(TimeSpan.FromDays(10)), "Closed");
            CreateAssessmentWithRetults(3, DateTime.Now, "Closed");

            Continuum.WebApi.Logic.AssessmentLogic assessmentLogic = CreateAssessmentLogic();

           int rating = assessmentLogic.GetCurrentLevelForTeam();

           Assert.IsFalse(rating == 5, "The latest assessment must be used to generate the rating.");      
        }

        private void CreateAssessmentWithRetults(int level, DateTime dateCreated, string status)
        {
            var assessmentResults = new List<Continuum.Data.AssessmentResult>();

            assessmentResults.Add(new Data.AssessmentResult() { Rating = level.ToString() });
            assessmentResults.Add(new Data.AssessmentResult() { Rating = level.ToString() });
            assessmentResults.Add(new Data.AssessmentResult() { Rating = level.ToString() });
            assessmentResults.Add(new Data.AssessmentResult() { Rating = level.ToString() });

            _mockContainer.Assessments.Add(new Data.Assessment()
            {
                DateCreated = dateCreated,
                AssessmentResults = assessmentResults,
                Status = new Data.AssessmentStatus() { Value = status }
            });
        }

        private WebApi.Logic.AssessmentLogic CreateAssessmentLogic()
        {
            var identity = new System.Security.Principal.GenericIdentity("TestUser");
            var principal = new System.Security.Principal.GenericPrincipal(identity, new string[] { });

            Continuum.WebApi.Logic.AssessmentLogic assessmentLogic = new WebApi.Logic.AssessmentLogic(_assessmentRepo, _teamRepository, _dimensionRepo, principal);
            return assessmentLogic;
        }

        [TestMethod]
        public void TestThatDeleteDeletesTeam()
        {
            var teamLogic = CreateTeamLogicForAdmin();

            var team = new Core.Models.Team() 
            {
                Id = 1,
                Name = Guid.NewGuid().ToString()
            };

            teamLogic.CreateTeam(team);

            teamLogic.DeleteTeam(team.Id);

            Assert.IsFalse(_mockContainer.Teams.Any(i => i.Id == team.Id), "Team was not deleted.");
        }

        private WebApi.Logic.TeamLogic CreateTeamLogicForAdmin()
        {
            return CreateTeamLogic(new string[] {"SiteAdmin" });
        }

        private WebApi.Logic.TeamLogic CreateTeamLogic(string[] roles)
        {
            var identity = new System.Security.Principal.GenericIdentity("TestUser");
            var principal = new System.Security.Principal.GenericPrincipal(identity, roles);
            var teamLogic = new WebApi.Logic.TeamLogic(_teamRepository, principal);
            return teamLogic;
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestThatDeleteForNonExistantTeamThrowsException()
        {
            var teamLogic = CreateTeamLogicForAdmin();

            teamLogic.DeleteTeam(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void TestThatOnlySiteAdminMayDeleteTeam()
        {
            var teamLogic = CreateTeamLogic(null);

            teamLogic.DeleteTeam(-1);
        }

        [TestMethod]
        public void TestThatTeamWithNoAssessmentResultsGetsDefaultRating()
        {

            CreateTeamWithMember();

            var assessmentResults = new List<Continuum.Data.AssessmentResult>();

            _mockContainer.Assessments.Add(new Data.Assessment()
            {
                DateCreated = DateTime.Now,
                AssessmentResults = assessmentResults,
                Status = new Data.AssessmentStatus() { Value = "Closed" }
            });

            var identity = new System.Security.Principal.GenericIdentity("TestUser");
            var principal = new System.Security.Principal.GenericPrincipal(identity, new string[] { });

            Continuum.WebApi.Logic.AssessmentLogic assessmentLogic = new WebApi.Logic.AssessmentLogic(_assessmentRepo, _teamRepository, _dimensionRepo, principal);

            int rating = assessmentLogic.GetCurrentLevelForTeam();

            Assert.IsTrue(rating == 1, "Rating must be 1 if there is not assessment info.");
        }

        [TestMethod]
        public void TestGetTeamMembersForTeam()
        {
            CreateTeamWithMember();

            var teamLogic = CreateTeamLogic(null);

            var members = teamLogic.GetTeamMembers(_mockContainer.Teams.First().Id);

            Assert.IsTrue(members.Count() == 1); 
        }


       
    }
}
