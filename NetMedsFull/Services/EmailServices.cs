﻿using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.Services
{
    public class EmailServices
    {
        public interface IEmailService
        {
            void Send(string to, string subject, string html);
        }

        public class EmailService : IEmailService
        {
            public void Send(string to, string subject, string html)
            {
                // create message
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("desteknetmeds@gmail.com"));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) { Text = html };

                // send email
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("desteknetmeds@gmail.com", "Netmed123");
                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }
    }
}
