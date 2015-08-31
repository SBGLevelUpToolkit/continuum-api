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
           
        }

        [TestMethod]
        public void TestThatCreatingADuplicateTeamThrowsException()
        {
            var teamRepo = new Data.Mocks.MockTeamRepo();

            teamRepo.TeamData.Add(new Data.Team() { Name = "Test Team"});

            TeamController teamController = new TeamController(teamRepo);
        
            WebApi.Models.Team newTeam = new WebApi.Models.Team() 
            {
                Name = teamRepo.All().First().Name
            };

            try
            {
                teamController.Put(newTeam);
                Assert.Fail("Must not create duplicate teams");
            }
            catch (HttpResponseException ex)
            {
                Assert.IsTrue(ex.Response.StatusCode == System.Net.HttpStatusCode.InternalServerError);
            }

        }

        [TestMethod]
        public void TestThatCreatingANewTeamIsAllowed()
        {
            var teamRepo = new Data.Mocks.MockTeamRepo();

            TeamController teamController = new TeamController(teamRepo);
            teamController.Request = _request;

            WebApi.Models.Team newTeam = new WebApi.Models.Team() 
            {
                Name = "Test Team"
            };

            var result = teamController.Put(newTeam);

            Assert.IsTrue(result.StatusCode == System.Net.HttpStatusCode.Created);
            Assert.IsTrue(teamRepo.All().Where(i => i.Name == newTeam.Name).Count() == 1);
        }

        [TestMethod]
        public void TestThatUserWhoCreatesTeamIsSetAsTeamAdmin()
        {
            var user = System.Security.Principal.WindowsIdentity.GetCurrent();

            var teamRepo = new Data.Mocks.MockTeamRepo();

            TeamController teamController = new TeamController(teamRepo);
            teamController.Request = _request;

            WebApi.Models.Team newTeam = new WebApi.Models.Team() 
            {
                Name = "Test Team"
            };

            teamController.Put(newTeam);

            var team = teamRepo.All().Where(i => i.Name == newTeam.Name).FirstOrDefault();
            Assert.IsNotNull(team, "Could not find new team");

            var teamMember = team.TeamMembers.Where(i => i.UserId == user.Name).FirstOrDefault();

            Assert.IsNotNull(teamMember, "User was not assigned to team.");

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
