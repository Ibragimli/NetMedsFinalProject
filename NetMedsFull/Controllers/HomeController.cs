using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetMedsFull.Models;
using NetMedsFull.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
                DiscountProducts = _context.Products.Include(x => x.ProductImages).Where(x => x.DiscountPercent > 0).ToList(),
                TrendSliders = _context.TrendSliders.Include(x => x.Product).ToList(),
                Sliders = _context.Sliders.Include(x => x.Product).ToList(),
                Categories = _context.Categories.Where(x => x.IsNav == true && x.IsDelete == false).ToList(),
                SbiLabs = _context.SbiLabs.ToList(),

            };
            return View(homeVM);
        }
    }
}
