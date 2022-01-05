using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebsterWebApp.Services
{
    public class Mail
    {
        private readonly Repository.IMailService mailServices;
        public Mail(Repository.IMailService mailServices)
        {
            this.mailServices = mailServices;
        }

        public Mail()
        {
        }

        public async Task SendMail(string toMail, string subject, string body) 
        {
            await mailServices.SendMail(toMail, subject, body);
        }
    }
}
