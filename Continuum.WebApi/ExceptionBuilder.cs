using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Continuum.WebApi
{
    public class ExceptionBuilder
    {
        public static HttpResponseException CreateException(string message, string reason, HttpStatusCode status)
        {
            var resp = new HttpResponseMessage(status)
            {
                Content = new StringContent(message),
                ReasonPhrase = reason
            };

            return new HttpResponseException(resp);
        }

        public static HttpResponseException CreateInternalServerError(string message, string reason)
        {
            return CreateException(message, reason, HttpStatusCode.InternalServerError);
        }

        public static HttpResponseException FromException(ApplicationException ex)
        {
            return CreateInternalServerError(ex.Message, "Application Error");
        }
    }
}