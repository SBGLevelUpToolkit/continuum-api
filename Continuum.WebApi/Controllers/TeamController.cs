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
        private readonly Data.ITeamRepo _teamRepo;
        private readonly Logic.TeamLogic _teamLogic;

        public TeamController(Data.ITeamRepo teamRepo) : base(teamRepo)
        {
            _teamRepo = teamRepo;
            _teamLogic = new Logic.TeamLogic(_teamRepo, CurrentUser == null ? this.User : CurrentUser);
        }

        [ApplicationExceptionFilter]
        public HttpResponseMessage Put(Models.Team team)
        {
            Models.Team result =_teamLogic.CreateTeam(team);

           var response = new HttpResponseMessage(HttpStatusCode.Created);
           response.Content = new ObjectContent(typeof(Models.Team), result, new JsonMediaTypeFormatter());

           return response;
        }

        [ApplicationExceptionFilter]
        public void Post(Models.Team team)
        {
            _teamLogic.JoinTeam(team);
        }

        public IEnumerable<Models.Team> Get()
        {
            return _teamLogic.ListTeams();
        }
       
    }
}
