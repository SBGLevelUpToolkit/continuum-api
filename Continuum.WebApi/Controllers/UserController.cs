using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Web.Http;
using Continuum.Data;
using Continuum.WebApi.Filters;
using Continuum.Core.Models; 

namespace Continuum.WebApi.Controllers
{
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly TeamRepo _teamRepo;
        private readonly Logic.TeamLogic _teamLogic;

        public UserController(TeamRepo teamRepository)
        {
            _teamRepo = teamRepository;
            _teamLogic = new Logic.TeamLogic(_teamRepo, CurrentUser == null ? this.User : CurrentUser);
        }

        /// <summary>
        /// Returns the details for the current user. 
        /// </summary>
        /// <returns></returns>
        public Continuum.Core.Models.User Get()
        {
            return _teamLogic.GetUserDetails(CurrentUser == null ? this.User.Identity.Name : CurrentUser.Identity.Name);
        }

        [ApplicationExceptionFilter]
        public void Put(Continuum.Core.Models.User user)
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
