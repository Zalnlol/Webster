using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebsterWebApp.Areas.Admin.Models;

namespace WebsterWebApp.Repository
{
    public interface IMailService
    {
        Task SendMail(MailContact mailContact);
    }
}
