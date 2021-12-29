using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebsterWebApp.Data;
using WebsterWebApp.Models;

namespace WebsterWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ExamController : Controller
    {
        //private readonly DatabaseContext _context;

        public IActionResult Index()
        {
            return View();
        }
    }
}
