using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace Continuum.WebApi.Filters
{
    public class ValidationResultFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is DbEntityValidationException)
            {
                var exception = actionExecutedContext.Exception as DbEntityValidationException;

                System.Text.StringBuilder builder = new System.Text.StringBuilder();

                foreach(var validationIssue in exception.EntityValidationErrors)
                {
                    foreach (var propertyError in validationIssue.ValidationErrors)
                    {
                        builder.AppendFormat("{0} [{1}] {2}\r\n", validationIssue.Entry.Entity.GetType().Name, propertyError.PropertyName, propertyError.ErrorMessage);
                    }   
                }

                actionExecutedContext.Response = ExceptionBuilder.CreateInternalServerError(builder.ToString(), "Validation").Response;
            }
        }
    }
}