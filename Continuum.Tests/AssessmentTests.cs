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
        private Continuum.Data.Mocks.MockContainer _mockContainer; 
        private Continuum.Data.AssessmentRepo _assessmentRepo;
        private Continuum.Data.TeamRepo _teamRepo;
        private Continuum.WebApi.Controllers.AssessmentController _assessmentController;
 
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
            _mockContainer = new Data.Mocks.MockContainer(); 
            _assessmentRepo = new Continuum.Data.AssessmentRepo(_mockContainer);
            _teamRepo = new Continuum.Data.TeamRepo(_mockContainer);

            var identity = new System.Security.Principal.GenericIdentity("alice@example.com");
            var princpal = new System.Security.Principal.GenericPrincipal(identity, new string[] { });
            Continuum.WebApi.Controllers.AssessmentController.CurrentUser = princpal;

            _assessmentController = new Continuum.WebApi.Controllers.AssessmentController(_assessmentRepo, _teamRepo);
            _assessmentController.User = princpal;

        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestThatGetWhenNoResultsThrowsException()
        {
            string user = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

            Data.Team team;
            Data.TeamMember teamMember;
            CreateTeamAndTeamMember(out team, out teamMember);

            _assessmentController.Get();
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestThatAttemptingToAccessAssessmentWithoutTeamThrowsException()
        {
            var identity = new System.Security.Principal.GenericIdentity("noteam@example.com");
            var princpal = new System.Security.Principal.GenericPrincipal(identity, new string[] { });

            Continuum.WebApi.Controllers.AssessmentController.CurrentUser = princpal;

            _assessmentController.User = princpal;

            _assessmentController.Get();
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestThatUpdateToClosedAssessmentThrowsException()
        {

            var team = new Data.Team();
            team.TeamMembers.Add(new Data.TeamMember() { UserId = TestUser });
            _mockContainer.Teams.Add(team);

            _mockContainer.Assessments.Add(new Data.Assessment()
            {
                Id = 1,
                Status = new Data.AssessmentStatus() { Value = "Closed" },
                Team = team
            });

            var result = new Continuum.WebApi.Models.AssessmentResult();
            result.AssessmentId = 1;

            _assessmentController.Put(new List<Continuum.WebApi.Models.AssessmentResult>() { result });
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestThatModeratingWithoutCreatingAssessmentThrowsException()
        {
            Data.Team team;
            Data.TeamMember teamMember;
            CreateTeamAndTeamMember(out team, out teamMember);

            var result = new Continuum.WebApi.Models.AssessmentResult();

            _assessmentController.Put(new List<Continuum.WebApi.Models.AssessmentResult>() { result });
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestThatCannotSetClosedOnOpenAssessment()
        {
            var assessmentItem = CreateOpenAssessment();

            _mockContainer.Teams.First().TeamMembers.First().IsAdmin = true;

            _assessmentController.Close();
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestThatModerationThrowsExceptionIfNoAssessment()
        {
            _assessmentController.Moderate();
            Assert.Fail();
        }
        
        [TestMethod]
        public void TestThatNewAssessmentIsCreatedWithOpenStatus()
        {
            Data.Team team;
            Data.TeamMember teamMember;
            CreateTeamAndTeamMember(out team, out teamMember);

            _assessmentController.Create();

            Assert.IsTrue(_mockContainer.Assessments.Count() == 1);
            Assert.IsTrue(_mockContainer.Assessments.First().Status.Value == "Open");
            Assert.IsTrue(_mockContainer.Assessments.First().TeamId == team.Id);
        }

        [TestMethod]
        public void TestThatUpdateToAssessmentDoesNotDuplicateItems()
        {
            var assessmentItem = CreateOpenAssessment();

            var item = new Continuum.WebApi.Models.AssessmentItem();

            _assessmentController.Post(new List<Continuum.WebApi.Models.AssessmentItem>() { assessmentItem });

            Assert.IsTrue(_mockContainer.Assessments.Count() == 1);
            Assert.IsTrue(_mockContainer.Assessments.First().AssessmentItems.Count() == 1);
            Assert.IsTrue(_mockContainer.Assessments.First().AssessmentItems.First().CapabilityAchieved == false);

        }

        private Continuum.WebApi.Models.AssessmentItem CreateOpenAssessment()
        {
            Data.Team team;
            Data.TeamMember teamMember;
            CreateTeamAndTeamMember(out team, out teamMember);

            var assessment = new Data.Assessment()
            {
                Team = team,
                Status = new Data.AssessmentStatus(){ Value = "Open"},
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
            _mockContainer.Assessments.Add(assessment);

            return new WebApi.Models.AssessmentItem()
            {
                AssesmentId = assessmentItem.Id,
                CapabilityAchieved = assessmentItem.CapabilityAchieved,
                CapabilityId = assessmentItem.CapabiltyId
            };
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


        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestThatCreateWhenOpenAssessmentThrowsException()
        {
            var assessmentItem = CreateOpenAssessment();

            _assessmentController.Create();
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestThatCreateWhenModeratedAssessmentThrowsException()
        {
            var assessmentItem = CreateOpenAssessment();

            _mockContainer.Assessments.First().Status.Value = "Moderating";

            _assessmentController.Create();
            Assert.Fail();
        }

        [TestMethod]
        public void TestThatCreateAssessmentResultIsCreated()
        {
            CreateOpenAssessment();

            _mockContainer.Assessments.First().Status.Value = "Moderating";

            var result = new Continuum.WebApi.Models.AssessmentResult();
            result.AssessmentId = _mockContainer.Assessments.First().Id;
            result.Rating = 3;

            _assessmentController.Put(new List<Continuum.WebApi.Models.AssessmentResult>() { result });

            Assert.IsTrue(_mockContainer.Assessments.First().AssessmentResults.Count == 1);
            Assert.IsTrue(_mockContainer.Assessments.First().AssessmentResults.First().Rating == result.Rating.ToString());
        }

        [TestMethod]
        public void TestThatModerateChangesStatus()
        {
            var assessmentItem = CreateOpenAssessment();

            _mockContainer.Teams.First().TeamMembers.First().IsAdmin = true;

            _assessmentController.Moderate();

            Assert.IsTrue(_mockContainer.Assessments.First().Status.Value == "Moderating");
        }
            


        [TestMethod]
        public void TestThatCloseChangesStatus()
        {
            var assessmentItem = CreateOpenAssessment();

            _mockContainer.Teams.First().TeamMembers.First().IsAdmin = true;
            _mockContainer.Assessments.First().Status.Value = "Moderating";
            _mockContainer.Assessments.First().Team.TeamMembers.First().IsAdmin = true;

            _assessmentController.Close();

            Assert.IsTrue(_mockContainer.Assessments.First().Status.Value  == "Closed");
        }
    }
}
