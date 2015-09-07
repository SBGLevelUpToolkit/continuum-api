using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Web.Http;
using Continuum.Data;
using Continuum.WebApi.Filters;

namespace Continuum.WebApi.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly ITeamRepo _teamRepo;
        private readonly Logic.TeamLogic _teamLogic;

        public UserController(ITeamRepo teamRepository)
        {
            _teamRepo = teamRepository;
            _teamLogic = new Logic.TeamLogic(_teamRepo, CurrentUser == null ? this.User : CurrentUser);
        }

        public Models.Team Get()
        {
            return _teamLogic.GetTeamForUser();
        }

        [ApplicationExceptionFilter]
        public void Put(Models.User user)
        {
            try
            {
                _teamLogic.UpdateUser(user);
            }
            catch (SecurityException ex)
            {
                throw ExceptionBuilder.CreateException(ex.Message, "Security", HttpStatusCode.Forbidden);
            }
        }
    }
}
