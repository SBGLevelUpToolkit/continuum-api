using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Continuum.Tests
{
    class MockMailProvider : IIdentityMessageService
    {
        public bool MailWasSent { get; private set; }

        public IdentityMessage Message { get; private set; }

        public System.Threading.Tasks.Task SendAsync(IdentityMessage message)
        {
            MailWasSent = true;
            Message = message;

            return Task.FromResult<object>(null);
        }
    }
}
