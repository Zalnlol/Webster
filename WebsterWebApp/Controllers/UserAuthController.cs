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

namespace WebsterWebApp.Controllers
{
    public class UserAuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly Repository.IMailService mailServices;


        public UserAuthController(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager, ApplicationDbContext _context, Repository.IMailService mailServices) 
        {
            this._context = _context;
            this._signInManager = _signInManager;
            this._userManager = _userManager;
            this.mailServices = mailServices;
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            Services.Mail mail = new Services.Mail();
            //await mail.SendMail(toMail, subject, body);

            //-------------------------------------------------------------

            //Set default login valid message to 'true', if it match, then the msg
            //is changed to empty, else it display Invalid login attempt.
            loginModel.LoginInValid = "true";

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginModel.Email,
                                                                     loginModel.Password,
                                                                     loginModel.RememberMe,
                                                                     lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    loginModel.LoginInValid = "";
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt");
                }
            }
            return PartialView("_UserLoginPartial", loginModel);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegistrationModel registrationModel)
        //public async Task<RegistrationModel> RegisterUser(RegistrationModel registrationModel)
        {
            registrationModel.RegistrationInValid = "true";
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
                    registrationModel.RegistrationInValid = "";
                    //Auto Log in if registration is success 
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return PartialView("_UserRegistrationPartial", registrationModel);
                }
                else 
                {
                    //code unreachable
                    AddErrorsToModelState(result);
                }
            }
            return PartialView("_UserRegistrationPartial", registrationModel);
        }

        [AllowAnonymous]
        public async Task<bool> UserNameExists(string userName) 
        {
            bool userNameExists = await _context.Users.AnyAsync(u => u.UserName.ToUpper() == userName.ToUpper());
            if (userNameExists == true) 
            {
                return true;
            }
            else { return false; }
        }

        private void AddErrorsToModelState(IdentityResult result) 
        {
            foreach (var error in result.Errors) 
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
