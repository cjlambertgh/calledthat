using System.Configuration;
using System.Net.Mail;

namespace EmailService
{
    public class SmtpMailService : IMailService
    {
        public void Send(string recipient, string subject, string body)
        {
            var mail = new MailMessage(ConfigurationManager.AppSettings["Mail.SMTP.From"], recipient);
            mail.Subject = subject;
            mail.Body = body;

            using (var client = new SmtpClient())
            {
                client.Host = ConfigurationManager.AppSettings["Mail.SMTP.Host"];
                client.Port = int.Parse(ConfigurationManager.AppSettings["Mail.SMTP.Port"]);
                client.Credentials = new System.Net.NetworkCredential(
                    ConfigurationManager.AppSettings["Mail.SMTP.User"],
                    ConfigurationManager.AppSettings["Mail.SMTP.Password"]
                    );

                client.Send(mail);
            }
        }
    }
}
