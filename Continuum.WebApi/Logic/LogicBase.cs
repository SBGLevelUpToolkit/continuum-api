using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Continuum.WebApi.Logic
{
    public abstract class LogicBase
    {
        protected readonly System.Security.Principal.IPrincipal _principal;

        public LogicBase(System.Security.Principal.IPrincipal principal)
        {
            if(principal == null) throw new ArgumentException("Value cannot be null.", "principal");
            _principal = principal;
        }

        protected string CurrentUserName
        {
            get
            {
                return _principal.Identity.Name;
            }
        }
    }
}