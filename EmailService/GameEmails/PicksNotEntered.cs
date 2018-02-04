using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService.GameEmails
{
    public class PicksNotEntered
    {
        private readonly IMailService mailService;
        private readonly string messageBody = "A new gameweek is about to begin and you haven't entered any predictions. <br /><br /> Click here to add your picks: ";
        private readonly string messageSubject = "You know the score - Predictions not entered.";

        public PicksNotEntered(IMailService mailService)
        {
            this.mailService = mailService;
        }

        public void Send(string recipient, string url)
        {
            var body = $"{messageBody}{url}";
            mailService.Send(recipient, messageSubject, body);
        }
    }
}
