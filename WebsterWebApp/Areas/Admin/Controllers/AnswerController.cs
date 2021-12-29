using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebsterWebApp.Areas.Admin.Models;

namespace WebsterWebApp.Areas.Admin.Controllers
{
    public class AnswerController : Controller
    {
        private readonly Data.DatabaseContext _db;

        public AnswerController(Data.DatabaseContext db)
        {
            this._db = db;
        }
    }
}
