using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetMedsFull.Areas.Manage.ViewModels;
using NetMedsFull.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.Areas.Manage.Controllers
{
    [Area("manage")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AdminLoginViewModel adminVM)
        {
            AppUser adminExist = await _userManager.FindByNameAsync(adminVM.Username);

            if (adminExist.IsAdmin == true && adminExist != null)
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                var result = await _signInManager.PasswordSignInAsync(adminExist, adminVM.Password, false, false);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Username or Passoword is incorrect!");
                    return View();
                }
                return RedirectToAction("index", "dashboard");
            }
            ModelState.AddModelError("", "Password or Username incorrect! ");
            return View();
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("login", "account");

        }
        //public async Task<IActionResult> CreateRole()
        //{
        //    var role1 = await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
        //    var role2 = await _roleManager.CreateAsync(new IdentityRole("Admin"));
        //    var role3 = await _roleManager.CreateAsync(new IdentityRole("Member"));

        //    AppUser SuperAdmin = new AppUser { FullName = "Super Admin", UserName = "SuperAdmin" };
        //    var admin = await _userManager.CreateAsync(SuperAdmin, "Admin123");
        //    var resultRole = await _userManager.AddToRoleAsync(SuperAdmin, "SuperAdmin");

        //    return Ok(resultRole);
        //} 
    }
}
