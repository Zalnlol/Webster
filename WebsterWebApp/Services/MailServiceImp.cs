using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using WebsterWebApp.Areas.Admin.Models;

namespace WebsterWebApp.Services
{
    public class MailServiceImp : Repository.IMailService
    {
        private readonly Settings.MailSettings contextMail;
        public MailServiceImp(IOptions<Settings.MailSettings> contextMail) 
        {
            this.contextMail = contextMail.Value;
        }
        public async Task SendMail(string toMail, string subject, string body)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(contextMail.Mail);
            email.To.Add(MailboxAddress.Parse(toMail));
            email.Subject = subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = body;

            //Set email attachment for this part below if needed
            //string filePath = @"C:\Users\jthie\OneDrive\Desktop\";
            //string fileName = filePath + mailContact.Attachments;
            //builder.Attachments.Add(fileName);

            email.Body = builder.ToMessageBody();
            SmtpClient smtp = new SmtpClient();
            smtp.Connect(contextMail.Host, contextMail.Port, MailKit.Security.SecureSocketOptions.StartTls);

            //Authentical email account
            smtp.Authenticate(contextMail.Mail, contextMail.Password);

            //Send mail
            await smtp.SendAsync(email);

            //Disconnect after send mail
            smtp.Disconnect(true);
        }
    }
}
