using System;
using System.Collections.Generic;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Continuum.Tests
{
    [TestClass]
    public class UserTests
    {

        private Continuum.Data.Mocks.MockContainer _mockContainer;
        private Continuum.Data.TeamRepo _teamRepo;
        private System.Security.Principal.IPrincipal _currentUser;
        private Continuum.WebApi.Controllers.UserController _controller;

        [TestInitializeAttribute]
        public void Setup()
        {
            _mockContainer = new Data.Mocks.MockContainer();
            _teamRepo = new Continuum.Data.TeamRepo(_mockContainer);

           
            var identity = new System.Security.Principal.GenericIdentity("alice@example.com");
            _currentUser = new System.Security.Principal.GenericPrincipal(identity, new string[] { });

            WebApi.Controllers.UserController.CurrentUser = _currentUser;

            _controller = new WebApi.Controllers.UserController(_teamRepo);
            _controller.User = _currentUser; 

        }

        [TestMethod]
        public void TestThatUserCannotUpdateAnotherUserDetails()
        {
            Continuum.Core.Models.User user = new Core.Models.User() { UserId = "test@test.com" };

            try
            {
                _controller.Put(user);
                Assert.Fail("Cannot update another user's details.");
            }
            catch(HttpResponseException ex)
            {
                Assert.IsTrue(ex.Response.StatusCode == System.Net.HttpStatusCode.Forbidden);
            }
        }

        [TestMethod]
        public void TestThatNewTeamsForUserAreAdded()
        {
            _mockContainer.Teams.Add(new Data.Team()
            {
                 Name = "Test Team",
                  Id = 100
            });

            var teams = new List<Continuum.Core.Models.Team>();
            teams.Add(new Core.Models.Team() { Id = 100 });

            Continuum.Core.Models.User user = new Core.Models.User()
            {
                UserId = _currentUser.Identity.Name, 
                Teams = teams
            };

            _controller.Put(user);

            var team = _mockContainer.Teams.Where(i => i.Id == 100).First();
            Assert.IsNotNull(team, "Could not retrieve team.");
            Assert.IsTrue(team.TeamMembers.Any(i => i.UserId == _currentUser.Identity.Name), "User was not added to the team.");
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestThatInvalidTeamIdThrowsException()
        {
     
            _mockContainer.Teams.Add(new Data.Team()
            {
                Name = "Test Team",
                Id = 100
            });

            var teams = new List<Continuum.Core.Models.Team>();
            teams.Add(new Core.Models.Team() { Id = 999 });

            Continuum.Core.Models.User user = new Core.Models.User()
            {
                UserId = _currentUser.Identity.Name,
                Teams = teams
            };

                _controller.Put(user);
                Assert.Fail(); 
        }

        [TestMethod]
        public void TestThatSystemDoesNotCreateDuplicateTeamMemberships()
        {
     
            _mockContainer.Teams.Add(new Data.Team()
            {
                Name = "Test Team",
                Id = 100
            });

            var teams = new List<Continuum.Core.Models.Team>();
            teams.Add(new Core.Models.Team() { Id = 100 });
            teams.Add(new Core.Models.Team() { Id = 100 });

            Continuum.Core.Models.User user = new Core.Models.User()
            {
                UserId = _currentUser.Identity.Name,
                Teams = teams
            };

            _controller.Put(user);

            var team = _mockContainer.Teams.Where(i => i.Id == 100).First();
            Assert.IsNotNull(team, "Could not retrieve team.");
            Assert.IsTrue(team.TeamMembers.Count(i => i.UserId == _currentUser.Identity.Name) == 1 , "Duplicate team entries created.");
        }

        [TestMethod]
        public void TestThatEmptyTeamCollectionThrowsException()
        {
            var teams = new List<Continuum.Core.Models.Team>();

            Continuum.Core.Models.User user = new Core.Models.User()
            {
                UserId = _currentUser.Identity.Name,
                Teams = teams
            };

            try
            {
                _controller.Put(user);
            }
            catch (HttpResponseException ex)
            {
                Assert.IsTrue(ex.Response.StatusCode == System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
