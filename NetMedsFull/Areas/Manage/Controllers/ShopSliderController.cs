using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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
    public class ShopSliderController : Controller
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _env;

        public ShopSliderController(DataContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {

            var shopSlider = _context.ShopSliders.ToList();
            if (shopSlider == null)
            {
                return RedirectToAction("notfounds", "error");
            }

            return View(shopSlider);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ShopSlider shopSlider)
        {
            if (shopSlider == null)
            {
                return RedirectToAction("notfounds", "error");
            }
            var existProduct = _context.Products.Any(x => x.Id == shopSlider.ProductId);

            if (!existProduct)
            {
                ModelState.AddModelError("ProductId", "Product not found!");
                return View();
            }
            if (shopSlider.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "ImageFile is required");
                return View();
            }
            else
            {
                if (shopSlider.ImageFile.ContentType != "image/png" && shopSlider.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "ImageFile is required");
                    return View();
                }
                if (shopSlider.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "ImageFile max size is 2MB");
                    return View();

                }
                shopSlider.Image = FileManager.Save(_env.WebRootPath, "uploads/shopsliders", shopSlider.ImageFile);
            }
            if (!ModelState.IsValid)
            {
                return View();
            }

            _context.ShopSliders.Add(shopSlider);
            _context.SaveChanges();

            return RedirectToAction("index", "dashboard");
        }



        public IActionResult Edit(int id)
        {
            var shopSlider = _context.ShopSliders.FirstOrDefault(x => x.Id == id);
            if (shopSlider == null)
            {
                return RedirectToAction("notfounds", "error");
            }

            return View(shopSlider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ShopSlider shopSlider)
        {
            var existshopSlider = _context.Sliders.FirstOrDefault(x => x.Id == shopSlider.Id);
            if (existshopSlider == null)
            {
                return RedirectToAction("notfounds", "error");
            }
            var existProduct = _context.Products.Any(x => x.Id == shopSlider.ProductId);

            if (!existProduct)
            {
                ModelState.AddModelError("ProductId", "Product not found!");
                return View(existshopSlider);
            }
            if (shopSlider.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "ImageFile is required");
                return View(existshopSlider);
            }
            else
            {
                if (shopSlider.ImageFile.ContentType != "image/png" && shopSlider.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "ImageFile is required");
                    return View(existshopSlider);
                }
                if (shopSlider.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "ImageFile max size is 2MB");
                    return View(existshopSlider);

                }
                FileManager.Delete(_env.WebRootPath, "uploads/shopsliders", existshopSlider.Image);
                existshopSlider.Image = FileManager.Save(_env.WebRootPath, "uploads/shopsliders", shopSlider.ImageFile);
            }
            if (!ModelState.IsValid)
            {
                return View(existshopSlider);
            }

            existshopSlider.ProductId = shopSlider.ProductId;

            _context.SaveChanges();
            TempData["Success"] = "Edit is succesfull!";
            return RedirectToAction("index", "slider");

        }

        public IActionResult Delete(int id)
        {
            var shopsliders = _context.ShopSliders.FirstOrDefault(x => x.Id == id);
            if (shopsliders == null)
            {
                return RedirectToAction("notfounds", "error");

            }
            var Image = shopsliders.Image;
            FileManager.Delete(_env.WebRootPath, "uploads/shopsliders", Image);


            _context.ShopSliders.Remove(shopsliders);
            _context.SaveChanges();
            TempData["Success"] = "Delete is succesfull!";

            return Ok();
        }
    }
}
