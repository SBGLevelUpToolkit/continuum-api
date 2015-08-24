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
        [TestMethod]
        public void TestThatUserCannotUpdateAnotherUserDetails()
        {
            var mockTeamRepo = new Data.Mocks.MockTeamRepo();

            Continuum.WebApi.Controllers.UserController controller = new WebApi.Controllers.UserController(mockTeamRepo);

            Continuum.WebApi.Models.User user = new WebApi.Models.User() { UserId = "test@test.com" };

            try
            {
                controller.Put(user);
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
            var mockTeamRepo = new Data.Mocks.MockTeamRepo();
            Continuum.WebApi.Controllers.UserController controller = new WebApi.Controllers.UserController(mockTeamRepo);

            mockTeamRepo.TeamData.Add(new Data.Team()
            {
                 Name = "Test Team",
                  Id = 100
            });

            var currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();

            var teams = new List<Continuum.WebApi.Models.Team>();
            teams.Add(new WebApi.Models.Team() { Id = 100 });

            Continuum.WebApi.Models.User user = new WebApi.Models.User()
            {
                UserId = currentUser.Name, 
                Teams = teams
            };

            controller.Put(user);

            var team = mockTeamRepo.TeamData.Where(i=>i.Id == 100).First();
            Assert.IsNotNull(team, "Could not retrieve team.");
            Assert.IsTrue(team.TeamMembers.Any(i => i.UserId == currentUser.Name), "User was not added to the team.");
        }

        [TestMethod]
        public void TestThatInvalidTeamIdThrowsException()
        {
            var mockTeamRepo = new Data.Mocks.MockTeamRepo();
            Continuum.WebApi.Controllers.UserController controller = new WebApi.Controllers.UserController(mockTeamRepo);

            mockTeamRepo.TeamData.Add(new Data.Team()
            {
                Name = "Test Team",
                Id = 100
            });

            var currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();

            var teams = new List<Continuum.WebApi.Models.Team>();
            teams.Add(new WebApi.Models.Team() { Id = 999 });

            Continuum.WebApi.Models.User user = new WebApi.Models.User()
            {
                UserId = currentUser.Name,
                Teams = teams
            };

            try
            {
                controller.Put(user);
                Assert.Fail(); 
            }
            catch (HttpResponseException ex)
            {
                Assert.IsTrue(ex.Response.StatusCode == System.Net.HttpStatusCode.InternalServerError);
            }
        }

        [TestMethod]
        public void TestThatSystemDoesNotCreateDuplicateTeamMemberships()
        {
            var mockTeamRepo = new Data.Mocks.MockTeamRepo();
            Continuum.WebApi.Controllers.UserController controller = new WebApi.Controllers.UserController(mockTeamRepo);

            mockTeamRepo.TeamData.Add(new Data.Team()
            {
                Name = "Test Team",
                Id = 100
            });

            var currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();

            var teams = new List<Continuum.WebApi.Models.Team>();
            teams.Add(new WebApi.Models.Team() { Id = 100 });
            teams.Add(new WebApi.Models.Team() { Id = 100 });

            Continuum.WebApi.Models.User user = new WebApi.Models.User()
            {
                UserId = currentUser.Name,
                Teams = teams
            };

            controller.Put(user);

            var team = mockTeamRepo.TeamData.Where(i => i.Id == 100).First();
            Assert.IsNotNull(team, "Could not retrieve team.");
            Assert.IsTrue(team.TeamMembers.Count(i => i.UserId == currentUser.Name) == 1 , "Duplicate team entries created.");
        }

        [TestMethod]
        public void TestThatEmptyTeamCollectionThrowsException()
        {
            var mockTeamRepo = new Data.Mocks.MockTeamRepo();
            Continuum.WebApi.Controllers.UserController controller = new WebApi.Controllers.UserController(mockTeamRepo);

            var currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();

            var teams = new List<Continuum.WebApi.Models.Team>();

            Continuum.WebApi.Models.User user = new WebApi.Models.User()
            {
                UserId = currentUser.Name,
                Teams = teams
            };

            try
            {
                controller.Put(user);
            }
            catch (HttpResponseException ex)
            {
                Assert.IsTrue(ex.Response.StatusCode == System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
