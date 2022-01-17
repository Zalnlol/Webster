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
using WebsterWebApp.Areas.Admin.Models;
using WebsterWebApp.Services;

namespace WebsterWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Repository.IMailService _mailService;

        public IActionResult Index()
        {
            return View();
        }

        public AdminController(ApplicationDbContext _context, UserManager<ApplicationUser> _userManager, Repository.IMailService _mailService)
        {
            this._context = _context;
            this._userManager = _userManager;
            this._mailService = _mailService;
        }

        // GET: Admin/UserList
        public IActionResult UserList()
        {
            var userList = from r in _context.UserRoles
                           join u in _context.Users
                           on r.UserId equals u.Id
                           where r.RoleId == "de5167aa-1f3f-454d-b6ca-6407d8ab0dbb"
                           select u;
            return View(userList);
        }

        // GET: Admin/AdminList
        public IActionResult AdminList()
        {
            var adminList = from r in _context.UserRoles
                            join u in _context.Users
                            on r.UserId equals u.Id
                            where r.RoleId == "79c11e1c-38c2-4e6b-a375-96261d4d65d5"
                            select u;
            return View(adminList);
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
                    _context.UserRoles.Add(new IdentityUserRole<string> { UserId = user.Id, RoleId = "de5167aa-1f3f-454d-b6ca-6407d8ab0dbb" });
                    _context.SaveChanges();
                    await _mailService.SendMail(registrationModel.Email, "Welcome to Webster", "Your account's password is: " + registrationModel.Password);
                    return RedirectToAction(nameof(Index));
                }
                return View(registrationModel);
            }
            return RedirectToAction(nameof(Index));
        }


        // GET: Admin/UserList/CreateCreateAdminAccount
        public IActionResult CreateAdminAccount()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdminAccount(RegistrationModel registrationModel)
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
                    _context.UserRoles.Add(new IdentityUserRole<string> { UserId = user.Id, RoleId = "79c11e1c-38c2-4e6b-a375-96261d4d65d5" });
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                return View(registrationModel);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
