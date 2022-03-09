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
    public class TrendSliderController : Controller
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _env;

        public TrendSliderController(DataContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {

            var trends = _context.TrendSliders.ToList();
            if (trends == null)
            {
                return RedirectToAction("notfounds", "error");
            }

            return View(trends);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TrendSlider trend)
        {
            if (trend == null)
            {
                return RedirectToAction("notfounds", "error");
            }
            var existProduct = _context.Products.Any(x => x.Id == trend.ProductId);

            if (!existProduct)
            {
                ModelState.AddModelError("ProductId", "Product not found!");
                return View();
            }
            if (trend.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "ImageFile is required");
                return View();
            }
            else
            {
                if (trend.ImageFile.ContentType != "image/png" && trend.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "ImageFile is required");
                    return View();
                }
                if (trend.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "ImageFile max size is 2MB");
                    return View();

                }
                trend.Name = FileManager.Save(_env.WebRootPath, "uploads/trendsliders", trend.ImageFile);
            }
            if (!ModelState.IsValid)
            {
                return View();
            }

            _context.TrendSliders.Add(trend);
            _context.SaveChanges();

            return RedirectToAction("index", "trendslider");
        }



        public IActionResult Edit(int id)
        {
            var trend = _context.TrendSliders.FirstOrDefault(x => x.Id == id);
            if (trend == null)
            {
                return RedirectToAction("notfounds", "error");
            }

            return View(trend);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TrendSlider trend)
        {
            var existTrend = _context.TrendSliders.FirstOrDefault(x => x.Id == trend.Id);
            if (existTrend == null)
            {
                return RedirectToAction("notfounds", "error");
            }
            var existProduct = _context.Products.Any(x => x.Id == trend.ProductId);

            if (!existProduct)
            {
                ModelState.AddModelError("ProductId", "Product not found!");
                return View(existTrend);
            }
            if (trend.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "ImageFile is required");
                return View(existTrend);
            }
            else
            {
                if (trend.ImageFile.ContentType != "image/png" && trend.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "ImageFile is required");
                    return View(existTrend);
                }
                if (trend.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "ImageFile max size is 2MB");
                    return View(existTrend);

                }
                FileManager.Delete(_env.WebRootPath, "uploads/trendsliders", existTrend.Name);
                existTrend.Name = FileManager.Save(_env.WebRootPath, "uploads/trendsliders", trend.ImageFile);
            }
            if (!ModelState.IsValid)
            {
                return View(existTrend);
            }

            existTrend.ProductId = trend.ProductId;

            _context.SaveChanges();
            TempData["Success"] = "Edit is succesfull!";
            return RedirectToAction("index", "trendslider");

        }

        public IActionResult Delete(int id)
        {
            var trend = _context.TrendSliders.FirstOrDefault(x => x.Id == id);
            if (trend == null)
            {
                return RedirectToAction("notfounds", "error");

            }
            var Image = trend.Name;
            FileManager.Delete(_env.WebRootPath, "uploads/trendsliders", Image);


            _context.TrendSliders.Remove(trend);
            _context.SaveChanges();
            TempData["Success"] = "Delete is succesfull!";

            return Ok();
        }
    }
}
