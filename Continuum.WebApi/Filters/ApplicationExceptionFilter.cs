using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace Continuum.WebApi.Filters
{
    public class ApplicationExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is ApplicationException) 
            {
                var applicationException = actionExecutedContext.Exception as ApplicationException;
                actionExecutedContext.Response = ExceptionBuilder.FromException(applicationException).Response;
            }
        }
    }
}