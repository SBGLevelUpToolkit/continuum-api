using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Routing;
using Continuum.WebApi.Filters;

namespace Continuum.WebApi.Controllers
{
    [Authorize]
    public class TeamController : ControllerBase
    {
        private readonly Data.TeamRepo _teamRepo;
   
        public TeamController(Data.TeamRepo teamRepo)
        {
            _teamRepo = teamRepo;
        }

        private Logic.TeamLogic TeamLogic
        {
            get
            {
                var user = CurrentUser == null ? this.User : CurrentUser;
                return new Logic.TeamLogic(_teamRepo, user);
            }
        }

        [ApplicationExceptionFilter]
        public HttpResponseMessage Put(Models.Team team)
        {
            if (ModelState.IsValid)
            {
                Models.Team result = TeamLogic.CreateTeam(team);
                var response = new HttpResponseMessage(HttpStatusCode.Created);
                response.Content = new ObjectContent(typeof(Models.Team), result, new JsonMediaTypeFormatter());
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [ApplicationExceptionFilter]
        public void Post(Models.Team team)
        {
            TeamLogic.JoinTeam(team);
        }

        public IEnumerable<Models.Team> Get()
        {
            return TeamLogic.ListTeams();
        }
       
    }
}
