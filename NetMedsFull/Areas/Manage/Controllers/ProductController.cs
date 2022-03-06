using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetMedsFull.Areas.Manage.ViewModels;
using NetMedsFull.Enums;
using NetMedsFull.Helpers;
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

            var product = _context.Products.Include(x => x.Comments).Include(x => x.Brand).Include(x => x.ProductImages).ToList();
            if (product == null)
            {
                return RedirectToAction("notfounds", "error");
            }

            return View(product);
        }

        public IActionResult Create()
        {

            ViewBag.Brand = _context.Brands.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            ViewBag.Brand = _context.Brands.ToList();
            if (product == null)
            {
                return RedirectToAction("notfounds", "error");
            }
            if (product.Name == null)
            {
                ModelState.AddModelError("Name", "Name is required");
                return View();
            }
            if (product.CostPrice <= 0)
            {
                ModelState.AddModelError("CostPrice", "CostPrice is required");
                return View();
            }
            if (product.SalePrice <= 0)
            {
                ModelState.AddModelError("SalePrice", "SalePrice is required");
                return View();
            }
            if (product.DiscountPercent <= 0 && product.DiscountPercent > 100)
            {
                ModelState.AddModelError("DiscountPercent", "DiscountPercent Range(1,100)");
                return View();
            }
            if (!_context.Brands.Any(x => x.Id == product.BrandId))
            {
                ModelState.AddModelError("BrandId", "BrandId not found");
                return View();
            }

            if (product.PosterImageFile == null)
            {
                ModelState.AddModelError("PosterImageFile", "PosterImageFile is required");
                return View();
            }
            else
            {
                if (product.PosterImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("PosterImageFile", "PosterImageFile max size is 2MB");
                    return View();

                }
                if (product.PosterImageFile.ContentType != "image/png" && product.PosterImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("PosterImageFile", "PosterImageFile is required");
                    return View();
                }
                ProductImage image = new ProductImage
                {
                    PosterStatus = true,
                    Products = product,
                    Image = FileManager.Save(_env.WebRootPath, "uploads/products", product.PosterImageFile)
                };
                _context.ProductImages.Add(image);
            }

            if (product.ImageFiles == null)
            {
                ModelState.AddModelError("ImageFiles", "PosterImageFile is required");
                return View();
            }
            else
            {
                foreach (var item in product.ImageFiles)
                {
                    if (item.Length > 2097152)
                    {
                        ModelState.AddModelError("ImageFiles", "ImageFiles max size is 2MB");
                        return View();

                    }
                    if (item.ContentType != "image/png" && item.ContentType != "image/jpeg")
                    {
                        ModelState.AddModelError("ImageFiles", "ImageFiles is required");
                        return View();
                    }
                    ProductImage image = new ProductImage
                    {
                        PosterStatus = false,
                        Products = product,
                        Image = FileManager.Save(_env.WebRootPath, "uploads/products", item)
                    };
                    _context.ProductImages.Add(image);
                }

            }

            if (!ModelState.IsValid)
            {
                return View();
            }
            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction("index", "dashboard");
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
