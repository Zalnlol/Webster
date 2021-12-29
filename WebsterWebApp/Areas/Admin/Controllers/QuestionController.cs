using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebsterWebApp.Areas.Admin.Models;

namespace WebsterWebApp.Areas.Admin.Controllers
{
    public class QuestionController : Controller
    {
        private readonly Data.DatabaseContext _db;

        public QuestionController(Data.DatabaseContext db)
        {
            this._db = db;
        }

        public IActionResult Index()
        {
            return View(_db.Questions.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Question question)
        {
            return View();
        }
    }
}
