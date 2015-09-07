using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Continuum.WebApi.Controllers
{
    public class ControllerBase : System.Web.Http.ApiController
    {
        public static System.Security.Principal.IPrincipal CurrentUser; 
    }
}