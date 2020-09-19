using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Project.Services
{
    public class MailService
    {
        private readonly SmtpClient _mailClient;
        private readonly string _fromAddress;

        public MailService(IMailSettings settings)
        {
            _mailClient = new SmtpClient
            {
                Host = settings.Host,
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(settings.MailAddress, settings.MailPassword),
                EnableSsl = true,
                Timeout = 20000
            };

            _fromAddress = settings.MailAddress;
        }

        public async void SendMailAsync(string toAddress, string subject, string message)
        {
            using var mailMessage = new MailMessage(_fromAddress, toAddress)
            {
                Subject = subject,
                Body = message
            };

            await _mailClient.SendMailAsync(mailMessage);
        }

    }
}
