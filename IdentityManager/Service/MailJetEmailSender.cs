using Mailjet.Client.TransactionalEmails;
using Mailjet.Client;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Mailjet.Client.Resources;

namespace IdentityManager.Service
{
    public class MailJetEmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public MailJetEmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MailjetClient client = new MailjetClient(_configuration["MailJet:ApiKey"], _configuration["MailJet:SecretKey"]);

            var mail = new TransactionalEmailBuilder()
               .WithFrom(new SendContact(_configuration["Email:From"], _configuration["Email:ApplicationName"]))
               .WithSubject(subject)
               .WithHtmlPart(htmlMessage)
               .WithTo(new SendContact(email))
               .Build();
            await client.SendTransactionalEmailAsync(mail);
        }
    }
}
