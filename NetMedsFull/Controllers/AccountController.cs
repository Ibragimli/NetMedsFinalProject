using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetMedsFull.Models;
using NetMedsFull.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(DataContext context,UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> Login(MemberLoginViewModel user)
        {
            var UserExists = await _userManager.FindByNameAsync(user.Username);
            if (UserExists == null)
            {
                ModelState.AddModelError("", "Username or Password is incorrect!");
                return View();
            }
            if (!UserExists.IsAdmin)
            {
                var result = await _signInManager.PasswordSignInAsync(UserExists, user.Password, false, false);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Username or Password is incorrect!");
                    return View();
                }
                return RedirectToAction("home", "index");
            }

            ModelState.AddModelError("", "Username or Password is incorrect!");
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(MemberRegisterViewModel user)
        {
            
            return View();
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        public IActionResult ChangePassword()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }
    }
}
