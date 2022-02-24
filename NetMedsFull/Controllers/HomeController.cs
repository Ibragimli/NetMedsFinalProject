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
                NewProducts = _context.Products.Include(x=>x.Brand).Include(x => x.ProductImages).Where(x => x.IsNew).ToList(),
                FavouriteProducts = _context.Products.Include(x=>x.Brand).Include(x=>x.ProductImages).Where(x=>x.IsTrending).ToList(),
            };
            return View(homeVM);
        }
    }
}
