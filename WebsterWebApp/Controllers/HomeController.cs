using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebsterWebApp.Data;
using WebsterWebApp.Models;

namespace WebsterWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager, ApplicationDbContext _context)
        {
            _logger = logger;
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            this._context = _context;

        }

        public IActionResult Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var adminRoleID = _userManager.GetUserAsync(User).Result?.Id;
                var adminLogin = _context.UserRoles.SingleOrDefault(t=>t.UserId.Equals(adminRoleID));
                if (adminLogin != null) 
                {
                    if (adminLogin.RoleId == "79c11e1c-38c2-4e6b-a375-96261d4d65d5") 
                    {
                        return RedirectToAction("Admin","Admin","Index");
                    }
                
                }
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
