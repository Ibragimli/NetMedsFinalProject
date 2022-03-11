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
    public class OrderSliderController : Controller
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _env;

        public OrderSliderController(DataContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            var orderSliders = _context.OrderSliders.ToList();
            if (orderSliders == null)
            {
                return RedirectToAction("notfounds", "error");
            }

            return View(orderSliders);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(OrderSlider orderSlider)
        {
            if (orderSlider == null)
            {
                return RedirectToAction("notfounds", "error");
            }
            var existProduct = _context.Products.Any(x => x.Id == orderSlider.ProductId);

            if (!existProduct)
            {
                ModelState.AddModelError("ProductId", "Product not found!");
                return View();
            }
            if (orderSlider.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "ImageFile is required");
                return View();
            }
            else
            {
                if (orderSlider.ImageFile.ContentType != "image/png" && orderSlider.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "ImageFile is required");
                    return View();
                }
                if (orderSlider.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "ImageFile max size is 2MB");
                    return View();

                }
                orderSlider.Image = FileManager.Save(_env.WebRootPath, "uploads/ordersliders", orderSlider.ImageFile);
            }
            if (!ModelState.IsValid)
            {
                return View();
            }

            _context.OrderSliders.Add(orderSlider);
            _context.SaveChanges();

            return RedirectToAction("index", "orderslider");
        }



        public IActionResult Edit(int id)
        {
            var orderSlider = _context.OrderSliders.FirstOrDefault(x => x.Id == id);
            if (orderSlider == null)
            {
                return RedirectToAction("notfounds", "error");
            }

            return View(orderSlider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(OrderSlider orderSlider)
        {
            var existSlider = _context.OrderSliders.FirstOrDefault(x => x.Id == orderSlider.Id);
            if (existSlider == null)
            {
                return RedirectToAction("notfounds", "error");
            }
            var existProduct = _context.Products.Any(x => x.Id == orderSlider.ProductId);

            if (!existProduct)
            {
                ModelState.AddModelError("ProductId", "Product not found!");
                return View(existSlider);
            }
            if (orderSlider.ImageFile != null)
            {

                if (orderSlider.ImageFile.ContentType != "image/png" && orderSlider.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "ImageFile is required");
                    return View(existSlider);
                }
                if (orderSlider.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "ImageFile max size is 2MB");
                    return View(existSlider);

                }
                FileManager.Delete(_env.WebRootPath, "uploads/ordersliders", existSlider.Image);
                existSlider.Image = FileManager.Save(_env.WebRootPath, "uploads/ordersliders", orderSlider.ImageFile);
            }
            if (!ModelState.IsValid)
            {
                return View(existSlider);
            }

            existSlider.ProductId = orderSlider.ProductId;

            _context.SaveChanges();
            TempData["Success"] = "Edit is succesfull!";
            return RedirectToAction("index", "orderslider");

        }

        public IActionResult Delete(int id)
        {
            var orderSlider = _context.OrderSliders.FirstOrDefault(x => x.Id == id);
            if (orderSlider == null)
            {
                return RedirectToAction("notfounds", "error");

            }
            var Image = orderSlider.Image;
            FileManager.Delete(_env.WebRootPath, "uploads/ordersliders", Image);


            _context.OrderSliders.Remove(orderSlider);
            _context.SaveChanges();
            TempData["Success"] = "Delete is succesfull!";

            return Ok();
        }
    }
}
