﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Routing;
using Continuum.Core.Models;
using Continuum.WebApi.Filters;

namespace Continuum.WebApi.Controllers
{
    [Authorize]
    public class TeamController : ControllerBase
    {
        private readonly Data.TeamRepo _teamRepo;
        private readonly Data.AssessmentRepo _assessmentRepo;
        private readonly Data.DimensionRepo _dimensionRepo;
   
        public TeamController(Data.TeamRepo teamRepo, Data.AssessmentRepo assessmentRepo, Data.DimensionRepo dimensionRepo)
        {
            _teamRepo = teamRepo;
            _assessmentRepo = assessmentRepo;
            _dimensionRepo = dimensionRepo;
        }

        private Logic.AssessmentLogic AssessmentLogic
        {
            get
            {
                var user = CurrentUser == null ? this.User : CurrentUser;
                return new Logic.AssessmentLogic(_assessmentRepo, _teamRepo, _dimensionRepo, user);
            }
        }

        private Logic.TeamLogic TeamLogic
        {
            get
            {
                var user = CurrentUser == null ? this.User : CurrentUser;
                return new Logic.TeamLogic(_teamRepo, user);
            }
        }

        [TeamAdminFilter]
        [ApplicationExceptionFilter]
        public IHttpActionResult Put(int id, Team team)
        {
            if (ModelState.IsValid)
            {
                if (TeamLogic.TeamExists(id))
                {
                    team.Id = id;
                    TeamLogic.UpdateTeam(team);
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        /// <summary>
        /// Adds a member to a team. If the member is already a member of the team the record is updated.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="teamMember"></param>
        /// <returns></returns>
        [TeamAdminFilter]
        [ApplicationExceptionFilter]
        [Route("api/team/{id}/members")]
        [HttpPost]
        public IHttpActionResult UpdateTeamMembers(int id, TeamMember teamMember)
        {
            if (ModelState.IsValid)
            {
                if (TeamLogic.TeamExists(id))
                {
                    TeamLogic.AddUserToTeam(id, teamMember);
                    return Ok();
                }
                else
                {
                    return NotFound(); 
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [ApplicationExceptionFilter]
        public IHttpActionResult Post(Team team)
        {
            if (ModelState.IsValid)
            {
                Team result = TeamLogic.CreateTeam(team);
                return Content<Team>(HttpStatusCode.Created, result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        /// <summary>
        /// Adds the current user to the team specified by <paramref name="id"/>
        /// </summary>
        /// <param name="id">The id of the team to join.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/team/{id}/join")]
        [ApplicationExceptionFilter]
        public IHttpActionResult JoinTeam(int id)
        {
            if (TeamLogic.TeamExists(id))
            {
                var team = TeamLogic.GetTeam(id);
                TeamLogic.JoinTeam(team);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Returns a list of teams.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Team> Get()
        {
            return TeamLogic.ListTeams();
        }

        /// <summary>
        /// Get the details of the specified team.
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="Core.Models.Team"/></returns>
        [ResponseTypeAttribute(typeof(Core.Models.Team))]
        public IHttpActionResult Get(int id)
        {
            if(TeamLogic.TeamExists(id))
            {
                var team = TeamLogic.GetTeam(id);

                team.CurrentLevel = AssessmentLogic.GetCurrentLevelForTeam();

                return Content<Core.Models.Team>(HttpStatusCode.OK, team);
            }
            else 
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Returns a list of members for the specified team. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/team/{id}/members")]
        [ResponseType(typeof(IEnumerable<Core.Models.TeamMember>))]
        public IHttpActionResult GetTeamMembers(int id)
        {
            if (TeamLogic.TeamExists(id))
            {
                return Content<IEnumerable<Core.Models.TeamMember>>(HttpStatusCode.OK, TeamLogic.GetTeamMembers(id));
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Delete the specified team member.
        /// 
        /// Will return 404 if the team or the team member do not exists.
        /// Will retirn 401 if the called does not have permissions to delete the team member.
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        [Authorize(Roles = "SiteAdmin")]
        [HttpDelete]
        [Route("api/team/{teamId}/members/{memberId}")]
        public IHttpActionResult DeleteTeamMember(int teamId, int memberId)
        {
            if (TeamLogic.TeamExists(teamId) && TeamLogic.TeamMemberExists(memberId))
            {
                TeamLogic.DeleteTeamMember(teamId, memberId);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }


        /// <summary>
        /// Returns a list of available avatars.
        /// </summary>
        /// <returns></returns>
        [Route("api/team/avatars")]
        public IEnumerable<Avatar> GetAvatars()
        {
            return TeamLogic.GetAvatars(); 
        }

        /// <summary>
        /// Deletes the specifified team.
        /// 
        /// Will return 404 - Not Found if the team does not exist.
        /// Will return 401 - Unauthorised if the caller does not have permission to delete the team.
        /// 
        /// </summary>
        /// <param name="id">Id of the team to delete.</param>
        /// <returns></returns>
        [Authorize(Roles="SiteAdmin")]
        [ApplicationExceptionFilter]
        public IHttpActionResult Delete(int id)
        {
            if (TeamLogic.TeamExists(id))
            {
                TeamLogic.DeleteTeam(id);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
       
    }
}
