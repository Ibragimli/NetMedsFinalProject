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
    public class SliderController : Controller
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(DataContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {

            var slider = _context.Sliders.ToList();
            if (slider == null)
            {
                return RedirectToAction("notfounds", "error");
            }

            return View(slider);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Slider slider)
        {
            if (slider == null)
            {
                return RedirectToAction("notfounds", "error");
            }
            var existProduct = _context.Products.Any(x => x.Id == slider.ProductId);

            if (!existProduct)
            {
                ModelState.AddModelError("ProductId", "Product not found!");
                return View();
            }
            if (slider.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "ImageFile is required");
                return View();
            }
            else
            {
                if (slider.ImageFile.ContentType != "image/png" && slider.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "ImageFile is required");
                    return View();
                }
                if (slider.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "ImageFile max size is 2MB");
                    return View();

                }
                slider.Image = FileManager.Save(_env.WebRootPath, "uploads/sliders", slider.ImageFile);
            }
            if (!ModelState.IsValid)
            {
                return View();
            }

            _context.Sliders.Add(slider);
            _context.SaveChanges();

            return RedirectToAction("index", "slider");
        }



        public IActionResult Edit(int id)
        {
            var slider = _context.Sliders.FirstOrDefault(x => x.Id == id);
            if (slider == null)
            {
                return RedirectToAction("notfounds", "error");
            }

            return View(slider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Slider slider)
        {
            var existSlider = _context.Sliders.FirstOrDefault(x => x.Id == slider.Id);
            if (existSlider == null)
            {
                return RedirectToAction("notfounds", "error");
            }
            var existProduct = _context.Products.Any(x => x.Id == slider.ProductId);

            if (!existProduct)
            {
                ModelState.AddModelError("ProductId", "Product not found!");
                return View(existSlider);
            }
            if (slider.ImageFile != null)
            {
                if (slider.ImageFile.ContentType != "image/png" && slider.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "ImageFile is required");
                    return View(existSlider);
                }
                if (slider.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "ImageFile max size is 2MB");
                    return View(existSlider);

                }
                FileManager.Delete(_env.WebRootPath, "uploads/sliders", existSlider.Image);
                existSlider.Image = FileManager.Save(_env.WebRootPath, "uploads/sliders", slider.ImageFile);
            }
            if (!ModelState.IsValid)
            {
                return View(existSlider);
            }

            existSlider.ProductId = slider.ProductId;

            _context.SaveChanges();
            TempData["Success"] = "Edit is succesfull!";
            return RedirectToAction("index", "slider");

        }

        public IActionResult Delete(int id)
        {
            var slider = _context.Sliders.FirstOrDefault(x => x.Id == id);
            if (slider == null)
            {
                return RedirectToAction("notfounds", "error");

            }
            var Image = slider.Image;
            FileManager.Delete(_env.WebRootPath, "uploads/sliders", Image);


            _context.Sliders.Remove(slider);
            _context.SaveChanges();
            TempData["Success"] = "Delete is succesfull!";

            return Ok();
        }
    }
}
