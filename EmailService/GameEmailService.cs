using EmailService.GameEmails;

namespace EmailService
{
    public class GameEmailService : IGameEmailService
    {
        private readonly IMailService mailService;

        public GameEmailService(IMailService mailService)
        {
            this.mailService = mailService;
        }
        public void SendGameweekOpenEmail(string recipient, string url)
        {
            var mailItem = new GameweekOpen(mailService);
            mailItem.Send(recipient, url);
        }
    }
}
