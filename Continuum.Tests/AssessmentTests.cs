using System;
using System.Collections.Generic;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Continuum.Tests
{
    [TestClass]
    public class AssessmentTests
    {
        private Continuum.Data.Mocks.MockAssessmentRepo _mockAssessmentRepo;
        private Continuum.Data.Mocks.MockTeamRepo _mockTeamRepo;
        private Continuum.WebApi.Controllers.AssessmentController _assessmentController;
        private Continuum.Data.Mocks.MockLookupRepo _mockLookups;

        public string TestUser
        {
            get
            {
                string user = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                return user;
            }
        }

        [TestInitializeAttribute]
        public void Setup()
        {
            _mockAssessmentRepo = new Continuum.Data.Mocks.MockAssessmentRepo();
            _mockTeamRepo = new Continuum.Data.Mocks.MockTeamRepo();
            _mockLookups = new Continuum.Data.Mocks.MockLookupRepo(); 
            _assessmentController = new Continuum.WebApi.Controllers.AssessmentController(_mockAssessmentRepo, _mockTeamRepo, _mockLookups);
        }

        [TestMethod]
        public void TestThatGetWhenNoResultsReturnsNotFound()
        {
            string user = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

            var team = new Data.Team();
            team.TeamMembers.Add(new Data.TeamMember() { UserId = user });
            _mockTeamRepo.TeamData.Add(team);

            try
            {
                _assessmentController.Get();
            }
            catch (HttpResponseException ex)
            {
                Assert.IsTrue(ex.Response.StatusCode == System.Net.HttpStatusCode.NotFound);
            }
           
        }

        [TestMethod]
        public void TestThatAttemptingToAccessAssessmentWithoutTeamThrowsException()
        {
            try
            {
                _assessmentController.Get();
            }
            catch (HttpResponseException ex)
            {
                Assert.IsTrue(ex.Response.StatusCode == System.Net.HttpStatusCode.InternalServerError);
            }
        }

        [TestMethod]
        public void TestThatUpdateToClosedAssessmentThrowsException()
        {

            var team = new Data.Team();
            team.TeamMembers.Add(new Data.TeamMember() { UserId = TestUser });
            _mockTeamRepo.TeamData.Add(team);

            _mockAssessmentRepo.Assessments.Add(new Data.Assessment() 
            {
                Id = 1,
                Status = new Data.AssessmentStatus() { Value = "Closed" },
                Team = team 
            });

            Data.AssessmentResult result = new Data.AssessmentResult();
            result.AssessmentId = 1;

            try
            {
                _assessmentController.Put(new List<Data.AssessmentResult>() { result });
            }
            catch (HttpResponseException ex)
            {
                Assert.IsTrue(ex.Response.StatusCode == System.Net.HttpStatusCode.InternalServerError);
            }

        }

        [TestMethod]
        public void TestThatModeratingWithoutCreatingAssessmentThrowsException()
        {
            var team = new Data.Team();
            team.TeamMembers.Add(new Data.TeamMember() { UserId = TestUser });
            _mockTeamRepo.TeamData.Add(team);

            Data.AssessmentResult result = new Data.AssessmentResult();

            try
            {
                _assessmentController.Put(new List<Data.AssessmentResult>() { result });
                Assert.Fail();
            }
            catch (HttpResponseException ex)
            {
                Assert.IsTrue(ex.Response.StatusCode == System.Net.HttpStatusCode.NotFound);
            }

        }

        [TestMethod]
        public void TestThatOnlyTeamAdminCanModerateAssessment()
        {
            var assessmentItem = CreateOpenAssessment();

            _mockTeamRepo.TeamData.First().TeamMembers.First().IsAdmin = false;

            try
            {
                _assessmentController.Moderate();
                Assert.Fail();
            }
            catch (HttpResponseException ex)
            {
                Assert.IsTrue(ex.Response.StatusCode == System.Net.HttpStatusCode.Forbidden);
            }

        }

        [TestMethod]
        public void TestThatCannotSetClosedOnOpenAssessment()
        {
            var assessmentItem = CreateOpenAssessment();

            _mockTeamRepo.TeamData.First().TeamMembers.First().IsAdmin = true;

            try
            {
                _assessmentController.Close();
                Assert.Fail(); 
            }
            catch (HttpResponseException ex)
            {
                Assert.IsTrue(ex.Response.StatusCode == System.Net.HttpStatusCode.InternalServerError);
            }
        }

        [TestMethod]
        public void TestThatOnlyTeamAdminCanCloseAssessment()
        {
            var assessmentItem = CreateOpenAssessment();

            _mockTeamRepo.TeamData.First().TeamMembers.First().IsAdmin = false;
            _mockAssessmentRepo.Assessments.First().Status = _mockLookups.GetLookupForValue<Data.AssessmentStatus>("Moderating");

            try
            {
                _assessmentController.Moderate();
                Assert.Fail();
            }
            catch (HttpResponseException ex)
            {
                Assert.IsTrue(ex.Response.StatusCode == System.Net.HttpStatusCode.Forbidden);
            }

        }

        [TestMethod]
        public void TestThatModerationThrowsExceptionIfNoAssessment()
        {
            try
            {
                _assessmentController.Moderate();
                Assert.Fail();
            }
            catch (HttpResponseException ex)
            {
                Assert.IsTrue(ex.Response.StatusCode == System.Net.HttpStatusCode.InternalServerError);
            }
        }
        
        [TestMethod]
        public void TestThatNewAssessmentIsCreatedWithOpenStatus()
        {
             var team = new Data.Team() { Id = 1 };
            var teamMember = new Data.TeamMember() { UserId = TestUser, Id = 1 };
            team.TeamMembers.Add(teamMember);
            _mockTeamRepo.TeamData.Add(team);

            _assessmentController.Create();

            Assert.IsTrue(_mockAssessmentRepo.Assessments.Count == 1);
            Assert.IsTrue(_mockAssessmentRepo.Assessments.First().Status.Equals(_mockLookups.GetLookupForValue<Data.AssessmentStatus>("Open")));
            Assert.IsTrue(_mockAssessmentRepo.Assessments.First().Team.Id == team.Id);
        }

        [TestMethod]
        public void TestThatUpdateToAssessmentDoesNotDuplicateItems()
        {
            var assessmentItem = CreateOpenAssessment();

            Data.AssessmentItem item = new Data.AssessmentItem();

            _assessmentController.Post(new List<Data.AssessmentItem>() { assessmentItem });

            Assert.IsTrue(_mockAssessmentRepo.Assessments.Count == 1);
            Assert.IsTrue(_mockAssessmentRepo.Assessments.First().AssessmentItems.Count() == 1);
            Assert.IsTrue(_mockAssessmentRepo.Assessments.First().AssessmentItems.First().CapabilityAchieved == false);

        }

        private Data.AssessmentItem CreateOpenAssessment()
        {
            var team = new Data.Team() { Id = 1 };
            var teamMember = new Data.TeamMember() { UserId = TestUser, Id = 1 };
            team.TeamMembers.Add(teamMember);
            _mockTeamRepo.TeamData.Add(team);


            var assessment = new Data.Assessment()
            {
                Team = team,
                Status = _mockLookups.GetLookupForValue<Data.AssessmentStatus>("Open"),
                Id = 1,
                TeamId = team.Id
            };

            var assessmentItem = new Data.AssessmentItem()
            {
                AssessmentId = 1,
                CapabilityAchieved = false,
                CapabiltyId = 1,
                Id = 1,
                TeamMemberId = teamMember.Id
            };

            assessment.AssessmentItems.Add(assessmentItem);

            _mockAssessmentRepo.Assessments.Add(assessment);
            return assessmentItem;
        }


        [TestMethod]
        public void TestThatCreateWhenOpenAssessmentThrowsException()
        {
            var assessmentItem = CreateOpenAssessment();

            try
            {
                _assessmentController.Create();
                Assert.Fail();
            }
            catch (HttpResponseException ex)
            {
                Assert.IsTrue(ex.Response.StatusCode == System.Net.HttpStatusCode.InternalServerError);
            }
        }

        [TestMethod]
        public void TestThatCreateWhenModeratedAssessmentThrowsException()
        {
            var assessmentItem = CreateOpenAssessment();

            _mockAssessmentRepo.Assessments.First().Status = _mockLookups.GetLookupForValue<Data.AssessmentStatus>("Moderating");

            try
            {
                _assessmentController.Create();
                Assert.Fail();
            }
            catch (HttpResponseException ex)
            {
                Assert.IsTrue(ex.Response.StatusCode == System.Net.HttpStatusCode.InternalServerError);
            }
        }

        [TestMethod]
        public void TestThatCreateAssessmentResultIsCreated()
        {
            CreateOpenAssessment();

            _mockAssessmentRepo.Assessments.First().Status = _mockLookups.GetLookupForValue<Data.AssessmentStatus>("Moderating");

            Data.AssessmentResult result = new Data.AssessmentResult();
            result.AssessmentId = _mockAssessmentRepo.Assessments.First().Id;
            result.Rating = "3";

            _assessmentController.Put(new List<Data.AssessmentResult>() { result });

            Assert.IsTrue(_mockAssessmentRepo.Assessments.First().AssessmentResults.Count == 1);
            Assert.IsTrue(_mockAssessmentRepo.Assessments.First().AssessmentResults.First().Rating == result.Rating);
        }

        [TestMethod]
        public void TestThatModerateChangesStatus()
        {
            var assessmentItem = CreateOpenAssessment();

            _mockTeamRepo.TeamData.First().TeamMembers.First().IsAdmin = true;

            _assessmentController.Moderate();

            Assert.IsTrue(_mockAssessmentRepo.Assessments.First().Status.Equals(_mockLookups.GetLookupForValue<Data.AssessmentStatus>("Moderating")));
        }
            


        [TestMethod]
        public void TestThatCloseChangesStatus()
        {
            var assessmentItem = CreateOpenAssessment();

            _mockTeamRepo.TeamData.First().TeamMembers.First().IsAdmin = true;
            _mockAssessmentRepo.Assessments.First().Status = _mockLookups.GetLookupForValue<Data.AssessmentStatus>("Moderating");

            _assessmentController.Close();

            Assert.IsTrue(_mockAssessmentRepo.Assessments.First().Status.Equals(_mockLookups.GetLookupForValue<Data.AssessmentStatus>("Closed")));
        }
    }
}
