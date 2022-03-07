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
            //if (product.Type != ProductType.Tablet || product.Type != ProductType.Liquid)
            //{
            //    ModelState.AddModelError("Type", "Type not found");
            //    return View();
            //}
            if (product.PosterImageFile == null)
            {
                ModelState.AddModelError("PosterImageFile", "PosterImageFile is required");
                return View();
            }
            else
            {
                if (product.PosterImageFile.ContentType != "image/png" && product.PosterImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("PosterImageFile", "PosterImageFile is required");
                    return View();
                }
                if (product.PosterImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("PosterImageFile", "PosterImageFile max size is 2MB");
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
                    if (item.ContentType != "image/png" && item.ContentType != "image/jpeg")
                    {
                        ModelState.AddModelError("ImageFiles", "ImageFiles is required");
                        return View();
                    }
                    if (item.Length > 2097152)
                    {
                        ModelState.AddModelError("ImageFiles", "ImageFiles max size is 2MB");
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


        public IActionResult Edit(int id)
        {
            ViewBag.Brand = _context.Brands.ToList();
            var product = _context.Products.Include(x => x.Brand).Include(x => x.ProductImages).FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return RedirectToAction("notfounds", "error");
            }

            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            ViewBag.Brand = _context.Brands.ToList();

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
            var productExist = _context.Products.Include(x => x.Brand).Include(x => x.ProductImages).FirstOrDefault(x => x.Id == product.Id);
            if (productExist == null)
            {
                return RedirectToAction("notfounds", "error");
            }


            if (product.PosterImageFile != null)
            {

                var posterImage = product.PosterImageFile;
                if (posterImage.ContentType != "image/png" && posterImage.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("PosterImageFile", "PosterImageFile content type only jpeg and png");
                    return View(productExist);
                }
                if (posterImage.Length > 2097152)
                {
                    ModelState.AddModelError("PosterImageFile", "PosterImageFile max size is 2MB");
                    return View(productExist);
                }
            }

            if (product.ImageFiles != null)
            {
                var images = product.ImageFiles;
                foreach (var item in images)
                {
                    if (item.ContentType != "image/png" && item.ContentType != "image/jpeg")
                    {
                        ModelState.AddModelError("ImageFiles", "ImageFiles is required");
                        return View(productExist);
                    }
                    if (item.Length > 2097152)
                    {
                        ModelState.AddModelError("ImageFiles", "ImageFiles max size is 2MB");
                        return View(productExist);
                    }

                }
            }

            if (product.PosterImageFile != null)
            {
                ProductImage Poster = productExist.ProductImages.FirstOrDefault(x => x.PosterStatus == true);

                if (Poster == null) return RedirectToAction("notfound", "pages");
                string filename = FileManager.Save(_env.WebRootPath, "uploads/products", product.PosterImageFile);

                FileManager.Delete(_env.WebRootPath, "uploads/products", Poster.Image);
                Poster.Image = filename;
            }


            productExist.ProductImages.RemoveAll(x => x.PosterStatus == false && !product.ProductImagesIds.Contains(x.Id));



            if (product.ImageFiles != null)
            {
                foreach (var file in product.ImageFiles)
                {
                    if (file.ContentType != "image/png" && file.ContentType != "image/jpeg")
                    {
                        continue;
                    }

                    if (file.Length > 2097152)
                    {
                        continue;
                    }

                    ProductImage image = new ProductImage
                    {
                        PosterStatus = false,
                        Image = FileManager.Save(_env.WebRootPath, "uploads/products", file)
                    };
                    if (productExist.ProductImages == null)
                        productExist.ProductImages = new List<ProductImage>();
                    productExist.ProductImages.Add(image);
                }
            }
            productExist.Name = product.Name;
            productExist.Country = product.Country;
            productExist.SalePrice = product.SalePrice;
            productExist.CostPrice = product.CostPrice;
            productExist.Description = product.Description;
            productExist.BrandId = product.BrandId;
            productExist.Type = product.Type;
            productExist.IsTrending = product.IsTrending;
            productExist.IsNew = product.IsNew;
            _context.SaveChanges();
            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            var PosterImage = product.ProductImages.FirstOrDefault(x => x.PosterStatus == true);
            var Images = product.ProductImages.FirstOrDefault(x => x.PosterStatus == false);

            FileManager.Delete(_env.WebRootPath, "uploads/products", PosterImage.Image);
            FileManager.Delete(_env.WebRootPath, "uploads/products", Images.Image);
            _context.Products.Remove(product);

            _context.SaveChanges();
            return Ok();
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
