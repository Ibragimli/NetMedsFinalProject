using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetMedsFull.Models;
using NetMedsFull.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NetMedsFull.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _context;

        public HomeController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeViewModel homeVM = new HomeViewModel
            {
                NewProducts = _context.Products.Include(x => x.ProductImages).Where(x => x.IsNew).ToList(),
                FavouriteProducts = _context.Products.Include(x => x.ProductImages).Where(x => x.IsTrending).ToList(),
                DiscountProducts = _context.Products.Include(x => x.ProductImages).Where(x => x.DiscountPercent > 0).Take(8).ToList(),
                TrendSliders = _context.TrendSliders.Include(x => x.Product).ToList(),
                Sliders = _context.Sliders.Include(x => x.Product).ToList(),
                Categories = _context.Categories.Where(x => x.IsNav == true && x.IsDelete == false).ToList(),
                SbiLabs = _context.SbiLabs.ToList(),
            };
            return View(homeVM);
        }

        [HttpPost]
        public IActionResult Subscribe(string email)
        {
            if (email == null)
            {
                TempData["error"] = "Email is required";
                return RedirectToAction("index", "home");
            }
            var subscribes = _context.Subscribes.ToList();
            if (subscribes.Any(x => x.Email == email))
            {
                TempData["error"] = "Email artiq subscribe olunub!";
                return RedirectToAction("index", "home");
            }
            bool result = Validate(email);
            if (result == true)
            {
                Subscribe subscribe = new Subscribe();
                subscribe.Email = email;
                _context.Subscribes.Add(subscribe);
                _context.SaveChanges();
                TempData["error"] = "Subscribe olduğunuz üçün təşşəkkürümüzü bildiririk";
                return RedirectToAction("index", "home");
            }
            TempData["error"] = "Email doğru deyil!";
            return RedirectToAction("index", "home");
        }
        private static bool Validate(string emailAddress)
        {
            string pattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
            //if email is valid
            if (Regex.IsMatch(emailAddress, pattern))
            {
                return true;
            }
            return false;
        }
    }
}
