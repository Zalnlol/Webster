using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebsterWebApp.Data;
using WebsterWebApp.Models;

namespace WebsterWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IActionResult Index()
        {
            return View();
        }

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/UserList
        public async Task<IActionResult> UserList()
        {
            return View(await _context.Users.ToListAsync());
        }
    }
}
