using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using EmailService;

namespace CalledThat.App_Start
{
    public class IdentityEmail : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            var mail = new HttpMailService();
            return Task.Run(() => mail.Send(message.Destination, message.Subject, message.Body));
        }
    }
}