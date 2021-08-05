using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using HelpDesk.Api.Model.Interfaces;
using Microsoft.Extensions.Options;

namespace HelpDesk.Api.Entities.Services
{
    public class EmailSender : IEmailSender
    {
        private SmtpClient Cliente { get; }
        private EmailSenderOptions Options { get; }

        public EmailSender(IOptions<EmailSenderOptions> options)
        {
            Options = options.Value;
            Cliente = new SmtpClient()
            {
                Host = Options.Host,
                Port = Options.Port,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(Options.Email, Options.Password),
                EnableSsl = Options.EnableSsl,
            };
        }

        public Task SendEmailAsync(string subject, string message, string imagePath= "")
        {
            var correo = new MailMessage(from: Options.Email, to: Options.Email, subject: subject, body: message);
            foreach (var address in Options.CC.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                correo.To.Add(address);
            };
            correo.IsBodyHtml = true;

            if (imagePath!= String.Empty)
            {
                correo.Attachments.Add(new Attachment(imagePath));
            }
            return Cliente.SendMailAsync(correo);
        }
    }
}
