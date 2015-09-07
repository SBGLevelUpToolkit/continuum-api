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
    public class TeamTests
    {
        HttpRequestMessage _request;

        Data.Mocks.MockContainer _mockContainer;
        Data.TeamRepository _teamRepository;

        
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
            _teamRepository = new Data.TeamRepository(_mockContainer);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestThatCreatingADuplicateTeamThrowsException()
        {
        
            _mockContainer.Teams.Add(new Data.Team() { Name = "Test Team"});

            TeamController teamController = new TeamController(_teamRepository);
        
            WebApi.Models.Team newTeam = new WebApi.Models.Team() 
            {
                Name = _mockContainer.Teams.First().Name
            };

                teamController.Put(newTeam);
                Assert.Fail("Must not create duplicate teams");
        }

        [TestMethod]
        public void TestThatCreatingANewTeamIsAllowed()
        {
  

            TeamController teamController = new TeamController(_teamRepository);
            teamController.Request = _request;

            WebApi.Models.Team newTeam = new WebApi.Models.Team() 
            {
                Name = Guid.NewGuid().ToString()
            };

            var result = teamController.Put(newTeam);

            Assert.IsTrue(result.StatusCode == System.Net.HttpStatusCode.Created);
            Assert.IsTrue(_mockContainer.Teams.Where(i => i.Name == newTeam.Name).Count() == 1);
        }

        [TestMethod]
        public void TestThatUserWhoCreatesTeamIsSetAsTeamAdmin()
        {
           /// var user = System.Security.Principal.WindowsIdentity.GetCurrent();

           var identity = new System.Security.Principal.GenericIdentity("TestUser");
           var princpal = new System.Security.Principal.GenericPrincipal(identity, new string[] { });

           TeamController.CurrentUser = princpal;
           TeamController teamController = new TeamController(_teamRepository);
           teamController.Request = _request;
           teamController.User = princpal;
           
            WebApi.Models.Team newTeam = new WebApi.Models.Team() 
            {
                Name = Guid.NewGuid().ToString()
            };

            var result = teamController.Put(newTeam);

            var team = _mockContainer.Teams.Where(i => i.Name == newTeam.Name).FirstOrDefault();
            Assert.IsNotNull(team, "Could not find new team");

            var teamMember = team.TeamMembers.Where(i => i.UserId == identity.Name).FirstOrDefault();

            string allUsers = String.Join(",",team.TeamMembers.Select(i => i.UserId + i.IsAdmin.ToString()).ToArray());


            Assert.IsNotNull(teamMember, "User was not assigned to team. The current user is " + identity.Name + " Current Users:" + allUsers);

            Assert.IsTrue(teamMember.IsAdmin, "User was not created as administrator.");

        }

        [TestMethod]
        public void TestThatTeamCreatedWithDefaultAvatar()
        {
            
        }

        [TestMethod]
        public void TestThatAddedNewUserIsAddedAndNotAdmin()
        {
            
        }
       
    }
}
