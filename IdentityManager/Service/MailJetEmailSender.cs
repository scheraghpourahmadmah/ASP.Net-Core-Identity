using Mailjet.Client;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;

namespace IdentityManager.Service
{
    public class MailJetEmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        public MailJetOptions _mailJetOptions;
        public MailJetEmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //MailjetClient client = new MailjetClient(_configuration["MailJet:ApiKey"], _configuration["MailJet:SecretKey"]);

            //var mail = new TransactionalEmailBuilder()
            //   .WithFrom(new SendContact(_configuration["Email:From"], _configuration["Email:ApplicationName"]))
            //   .WithSubject(subject)
            //   .WithHtmlPart(htmlMessage)
            //   .WithTo(new SendContact(email))
            //   .Build();
            //var r = await client.SendTransactionalEmailAsync(mail);



            _mailJetOptions = _configuration.GetSection("MailJet").Get<MailJetOptions>();

            MailjetClient client = new MailjetClient(_mailJetOptions.ApiKey, _mailJetOptions.SecretKey)
            {
                Version = ApiVersion.V3_1,
            };
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
             .Property(Send.Messages, new JArray {
             new JObject {
      {
       "From",
       new JObject {
        {"Email", _configuration["Email:From"]},
        {"Name", "Benspark"}
       }
      }, {
       "To",
       new JArray {
        new JObject {
         {
          "Email",
          email
         }, {
          "Name",
          "Ben"
         }
        }
       }
      }, {
       "Subject",
      subject
      },  {
       "HTMLPart",
       htmlMessage
      },
     }
             });
           var r = await client.PostAsync(request);
        }
    }
}
