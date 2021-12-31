using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebsterWebApp.Areas.Admin.Models;

namespace WebsterWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AnswerController : Controller
    {
        private readonly Data.ApplicationDbContext _db;

        public AnswerController(Data.ApplicationDbContext db)
        {
            this._db = db;
        }

        public IActionResult Index()
        {
            return View(_db.Answers.ToList());
        }
    }
}
