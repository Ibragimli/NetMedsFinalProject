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
    public class ProductController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ProductController(DataContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Detail()
        {
            return View();
        }
        public IActionResult Shop()
        {
            return View();
        }

        public async Task<IActionResult> AddBasket(int productId)
        {
            if (!_context.Products.Any(x => x.Id == productId))
            {
                return RedirectToAction("error", "error");
            }
            BasketViewModel basketData = null;
            AppUser user = null;
            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }

            //if (user != null && user.IsAdmin == false)
            //{

            //}

            return Ok();
        }
    }
}
