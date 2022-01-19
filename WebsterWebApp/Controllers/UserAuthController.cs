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
    }
}
