using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace SendingMail.Services
{
    public class EmailSender: IEmailSender
    {
        protected IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Odeslání emailu
        /// </summary>
        /// <param name="email">emailová adresa příjemce</param>
        /// <param name="subject">předmět mailu</param>
        /// <param name="text">plain textová podoba obsahu</param>
        /// <returns>nic</returns>
        public async Task SendEmailAsync(string email, string subject, string text)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_configuration["EmailSender:FromName"], _configuration["EmailSender:From"]));
            message.To.Add(new MailboxAddress(email, email));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = text;
            bodyBuilder.HtmlBody = text;

            message.Body = bodyBuilder.ToMessageBody();

            if (Int32.TryParse(_configuration["EmailSender:Port"], out int port) == false) port = 0;
            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                await client.ConnectAsync(_configuration["EmailSender:Server"], port, MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable);
                await client.AuthenticateAsync(_configuration["EmailSender:Username"], _configuration["EmailSender:Password"]);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
