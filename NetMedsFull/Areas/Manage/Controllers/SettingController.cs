using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetMedsFull.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class SettingController : Controller
    {
        private readonly DataContext _context;

        public SettingController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var settings = _context.Settings.ToList();
            if (settings == null)
            {
                return RedirectToAction("notfounds", "error");
            }
            return View(settings);
        }
        public IActionResult Edit(int id)
        {
            var settings = _context.Settings.ToList().FirstOrDefault(x => x.Id == id);
            if (settings == null)
            {
                return RedirectToAction("notfounds", "error");
            }

            return View(settings);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Setting setting)
        {
            var settingExist = _context.Settings.FirstOrDefault(x => x.Id == setting.Id);
            if (settingExist == null)
            {
                return RedirectToAction("notfounds", "error");
            }

            if (setting.Value == null)
            {
                ModelState.AddModelError("Value", "Value is required");
                return View(settingExist);
            }

           

            if (!ModelState.IsValid)
            {
                return View(settingExist);
            }

            settingExist.Value = setting.Value;

            _context.SaveChanges();
            TempData["Success"] = "Edit is succesfull!";
            return RedirectToAction("index");
        }
    }
}
