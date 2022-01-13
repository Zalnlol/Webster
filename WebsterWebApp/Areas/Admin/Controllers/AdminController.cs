using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public IActionResult Index()
        {
            return View();
        }

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> _userManager)
        {
            _context = context;
            this._userManager = _userManager;
        }

        // GET: Admin/UserList
        public async Task<IActionResult> UserList()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Admin/UserList/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegistrationModel registrationModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = registrationModel.Email,
                    Email = registrationModel.Email,
                    PhoneNumber = registrationModel.PhoneNumber,
                    FirstName = registrationModel.FirstName,
                    LastName = registrationModel.LastName,
                };
                //Create method built into UserManager as part of Identity framework
                var result = await _userManager.CreateAsync(user, registrationModel.Password);
                if (result.Succeeded) 
                {
                    return RedirectToAction(nameof(Index));
                }
                return View(registrationModel);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
