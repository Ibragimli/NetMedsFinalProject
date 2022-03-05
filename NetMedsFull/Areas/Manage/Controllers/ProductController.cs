using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetMedsFull.Areas.Manage.ViewModels;
using NetMedsFull.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class ProductController : Controller
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<AppUser> _userManager;

        public ProductController(DataContext context, IWebHostEnvironment env, UserManager<AppUser> userManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
        }
        public IActionResult Index()
        {

            var product = _context.Products.Include(x=>x.Comments).Include(x => x.Brand).Include(x => x.ProductImages).ToList();
            if (product == null)
            {
                return RedirectToAction("notfounds", "error");
            }

            return View(product);
        }
        
        public IActionResult Create()
        {
            ProductCreateViewModel createVM = new ProductCreateViewModel
            {
                Product = new Product(),
                Brands = _context.Brands.ToList(),
            };
            return View(createVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (product == null)
            {
                return RedirectToAction("notfounds", "error");
            }
            return Ok(product);
        }

        public IActionResult Comments(int id)
        {
            List<Comment> comments = _context.Comments.Include(x => x.Product).Where(x => x.ProductId == id).ToList();
            if (comments == null)
            {
                return RedirectToAction("notfounds", "error");
            }
            return View(comments);
        }
    }
}
