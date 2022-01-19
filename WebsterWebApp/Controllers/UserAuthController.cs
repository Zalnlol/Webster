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
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebsterWebApp.Areas.Admin.Models;

namespace WebsterWebApp.Controllers
{
    public class UserAuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly Repository.IMailService _mailServices;
        public readonly IConfiguration _configuration;


        public UserAuthController(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager, ApplicationDbContext _context, Repository.IMailService _mailServices, IConfiguration _configuration) 
        {
            this._context = _context;
            this._signInManager = _signInManager;
            this._userManager = _userManager;
            this._mailServices = _mailServices;
            this._configuration = _configuration;
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
                    HttpContext.Session.SetString("Mail", loginModel.Email);

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
            //HttpContext.Session.Remove("Mail");
            //return BadRequest(HttpContext.Session.GetString("Mail"));



            await _signInManager.SignOutAsync();

            if (returnUrl != null)
            {
           
                return LocalRedirect(returnUrl);
            }
            else
            {
                HttpContext.Session.Remove("Mail");
                HttpContext.Session.Remove("count");
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
                    HttpContext.Session.SetString("Mail", registrationModel.Email);

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

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSetting:Secret").Value);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.NameIdentifier, user.Email.ToString()),
                    new Claim(ClaimTypes.Name, user.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public IActionResult PersonalPage()
        {


            var exam = _context.Exams.ToList();

            var ExamUserId = _context.Users.SingleOrDefault(t => t.UserName.Equals(HttpContext.Session.GetString("Mail"))).Id;

            var list = _context.ExamUsers.Where(s => s.UserId.Equals(ExamUserId)).ToList();


            var ds1 = (from s in list
                       join t in exam
                       on s.ExamId.ToString() equals t.ExamId.ToString()
                       select t
                      ).ToList();
            List<Models.Exam> examuser = new List<Models.Exam>();
            List<Models.ResultsModel> result = new List<Models.ResultsModel>();


            foreach (var item in ds1)
            {
                var t = _context.Results.SingleOrDefault(t => t.ExamId == item.ExamId && t.IdUser.Equals(ExamUserId) && t.TimeTech != 0);

                if (t != null)
                {
                    examuser.Add(item);
                    result.Add(t);
                }
                else
                {
                }

            }

            ViewBag.ds = examuser.ToArray();
            ViewBag.ds1 = result.ToArray();


            return View();
        }


        public IActionResult ChangePassword()
        {


            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(string currentpass, string newpass)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = _userManager.GetUserAsync(User).Result?.Email,
                Email = _userManager.GetUserAsync(User).Result?.Email,
                PhoneNumber = _userManager.GetUserAsync(User).Result?.PhoneNumber,
                FirstName = _userManager.GetUserAsync(User).Result?.FirstName,
                LastName = _userManager.GetUserAsync(User).Result?.LastName,
            };

            var result =  _userManager.ChangePasswordAsync(user, currentpass,newpass);

            if (result.Result.Succeeded)
            {
                return RedirectToAction("PersonalPage");
            }
            else
            {
                ViewBag.msg = "We can't change the password, please check again";
                return View();
            }
        }
    }
}
