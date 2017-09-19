using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService
{
    public class HttpMailService : IMailService
    {
        public void Send(string recipient, string subject, string body)
        {
            var client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v3");
            client.Authenticator =
            new HttpBasicAuthenticator("api",
                                ConfigurationManager.AppSettings["ApiKey"]);
            RestRequest request = new RestRequest();
            request.AddParameter("domain", ConfigurationManager.AppSettings["Domain"], ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", $"{ConfigurationManager.AppSettings["SenderName"]} <{ConfigurationManager.AppSettings["SenderAddress"]}>");
            request.AddParameter("to", recipient);
            request.AddParameter("subject", subject);
            request.AddParameter("text", body);
            request.Method = Method.POST;
            var response = client.Execute(request);
        }
    }
}
