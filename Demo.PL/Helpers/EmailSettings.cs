using Demo.DAL.Models;
using Demo.PL.Settings;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Helpers
{
    public class EmailSettings : IEmailSettings
    {

        //public static void SendEmail(Email email)
        //{
        //	var client = new SmtpClient("smtp.gmail.com", 587);
        //	client.Credentials = new NetworkCredential("Abdo@gmail.com", "01140988201");
        //	client.EnableSsl = true;
        //	client.Send("Abdo@gmail.com", email.Recipients, email.Subject, email.Body);

        //}

        private readonly MailSettings _options;
        public EmailSettings(IOptions<MailSettings> options)
        {
            _options = options.Value;
        }
        public void SendEmail(Email email)
        {
            var mail = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_options.Email),
                Subject = email.Subject,
            };

            mail.To.Add(MailboxAddress.Parse(email.Recipients));
            mail.From.Add(new MailboxAddress(_options.DisplayName, _options.Email));

            var builder = new BodyBuilder();
            builder.TextBody = email.Body;

            mail.Body = builder.ToMessageBody();

            //using var smtp = new SmtpClient 
            //{ 
            //    Host = _options.Host,
            //    Port = _options.Port,
            //    EnableSsl = true
            //};
            //smtp.Connect(_options.Host, _options.Port, SecureSocketOptions.StartTls);

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_options.Host, _options.Port, SecureSocketOptions.StartTls);

            smtp.Authenticate(_options.Email, _options.Password);
            smtp.Send(mail);

            smtp.Disconnect(true);

        }
    }
}
