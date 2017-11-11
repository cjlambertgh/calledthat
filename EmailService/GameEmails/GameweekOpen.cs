namespace EmailService.GameEmails
{
    public class GameweekOpen
    {
        private readonly IMailService mailService;
        private readonly string messageBody = "A new gameweek has been added. <br /><br /> Click here to add your picks: ";
        private readonly string messageSubject = "You know the score - A new gameweek has been added.";

        public GameweekOpen(IMailService mailService)
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
