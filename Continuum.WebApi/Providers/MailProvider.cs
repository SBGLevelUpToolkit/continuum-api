﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using SendGrid;

namespace Continuum.WebApi.Providers
{
    public class MailProvider : IIdentityMessageService
    {
        public System.Threading.Tasks.Task SendAsync(IdentityMessage message)
        {

            string userName = System.Configuration.ConfigurationManager.AppSettings["SendgridApiKey"];
            string password = System.Configuration.ConfigurationManager.AppSettings["SendgridApiPwd"];

            var mail = new SendGridMessage();
            mail.From = new MailAddress("registration@continuum.net");

            string ccList = System.Configuration.ConfigurationManager.AppSettings["EmailCCList"];
            if (!String.IsNullOrEmpty(ccList))
            {
                var ccAddressess = ccList.Split(';').Select(i=> new MailAddress(i)).ToArray();
                mail.Cc =  ccAddressess;
            }
            
            mail.To = new MailAddress[] { new MailAddress(message.Destination) };
            mail.Subject = message.Subject;
            mail.Html = message.Body;
          
            var credentials = new NetworkCredential(userName, password);
            var transportWeb = new Web(credentials);
            return transportWeb.DeliverAsync(mail);
        }
    }
}