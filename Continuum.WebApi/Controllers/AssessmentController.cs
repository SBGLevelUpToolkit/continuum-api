﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Continuum.Data;

namespace Continuum.WebApi.Controllers
{
    [Authorize]
    public class AssessmentController : ApiController
    {
        private readonly Data.IAssessmentRepo _assessmentRepo;
        private readonly Data.ITeamRepo _teamRepo;

        public AssessmentController(Data.IAssessmentRepo assessmentRepo, Data.ITeamRepo teamRepo)
        {
            _assessmentRepo = assessmentRepo;
            _teamRepo = teamRepo;
        }

        public Models.Assessment Get()
        {
            var team = GetTeamForUser();

            var assessment = _assessmentRepo.GetCurrentAssessmentForTeam(team.Id);

            if (assessment == null)
            {
                throw ExceptionBuilder.CreateException("No assessment data is available for this team.", "Not Found.", HttpStatusCode.NotFound);
            }

            var result = new Models.Assessment()
            {
                Id = assessment.Id,
                Status = assessment.Status.Value
            };

            return result;
        }

        private Data.Team GetTeamForUser()
        {
            var team = _teamRepo.GetTeamForUser(User.Identity.Name).FirstOrDefault();
            if (team == null)
            {
                throw ExceptionBuilder.CreateInternalServerError(String.Format("{0} is not a member of a team.", User.Identity.Name), "No Team.");
            }
            return team;
        }

        public void Put(IEnumerable<Models.AssessmentResult> assessmentResults)
        {
            var team = GetTeamForUser();
            var assessment = GetAssessmentForTeam(team);

            try
            {
                _assessmentRepo.UpdateModerationResultForAssessment(assessment, assessmentResults.Select(i=>
                new Data.AssessmentResult()
                {
                     AssessmentId = i.AssessmentId,
                      DimensionId = i.DimensionId, 
                       Rating = i.Rating.ToString()
                }));
                _assessmentRepo.SaveChanges();
            }
            catch (ApplicationException ex)
            {
                throw ExceptionBuilder.FromException(ex);
            }

        }

        public void Post(IEnumerable<Models.AssessmentItem> assessmentItems)
        {
            var team = GetTeamForUser();
            var assessment = GetAssessmentForTeam(team);
            var teamMember = team.TeamMembers.Where(i => i.UserId == User.Identity.Name).First();

            try
            {
                _assessmentRepo.UpdateCapabilityForAssessment(assessment,
                    assessmentItems.Select(i => new Data.AssessmentItem()
                    {
                        AssessmentId = i.AssesmentId,
                        CapabilityAchieved = i.CapabilityAchieved,
                        CapabiltyId = i.CapabilityId,
                        TeamMemberId = teamMember.Id
                    }));

                _assessmentRepo.SaveChanges();
            }
            catch (ApplicationException ex)
            {
                throw ExceptionBuilder.FromException(ex);
            }

        }

        private Data.Assessment GetAssessmentForTeam(Data.Team team)
        {
            var assessment = _assessmentRepo.GetCurrentAssessmentForTeam(team.Id);

            CheckAssessmentExists(assessment);

            return assessment;
        }

        [Route("api/assessment/create")]
        public void Create()
        {
            try
            {
                var team = GetTeamForUser();
                _assessmentRepo.CreateAssessmentForTeam(team);
                _assessmentRepo.SaveChanges();
            }
            catch (ApplicationException ex)
            {
                throw ExceptionBuilder.FromException(ex);
            }
        }

        [Route("api/assessment/moderate")]
        [Filters.TeamAdminFilter]
        public void Moderate()
        {
            var team = GetTeamForUser();
            var assessment = GetAssessmentForTeam(team);

            _assessmentRepo.StartModeration(assessment);
            _assessmentRepo.SaveChanges();
        }

        private static void CheckAssessmentExists(Assessment assessment)
        {
            if (assessment == null)
            {
                throw ExceptionBuilder.CreateInternalServerError("There are no assessments for this team.", "No Assessment.");
            }
        }

        [Route("api/assessment/close")]
        [Filters.TeamAdminFilter]
        public void Close()
        {
            var team = GetTeamForUser();
            var assessment = GetAssessmentForTeam(team);

            try
            {
                _assessmentRepo.CloseAssessment(assessment);
                _assessmentRepo.SaveChanges();
            }
            catch (ApplicationException ex)
            {
                throw ExceptionBuilder.FromException(ex);
            }
        }
    }
}
