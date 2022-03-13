using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetMedsFull.Models;
using NetMedsFull.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static NetMedsFull.Services.EmailServices;

namespace NetMedsFull.Controllers
{
    public class LabController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;

        public LabController(DataContext context, UserManager<AppUser> userManager, IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }
        public IActionResult LabTest()
        {

            //LabTestViewModel labTestVM = new LabTestViewModel
            //{
            //    Labtest = _context.LabTests,
            //   LabTestPrice = _context.LabTests.Where(x=>x.Id ==  )
            //};
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LabTest(LabTest labTest)
        {

            //LabTest existLabTest = labTest;
            //return Ok(existLabTest);

            if (labTest == null)
            {
                return RedirectToAction("error", "error");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            LabTestPostViewModel labTestVM = new LabTestPostViewModel
            {
                Email = labTest.Email,
                Fullname = labTest.Fullname,
                Rendezvous = labTest.Rendezvous,
                LabTestPriceId = labTest.LabTestPriceId,

            };
            labTest.LabStatus = Enums.LabStatus.Waiting;
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                labTest.AppUserId = user.Id;
                TempData["LabUser"] = user.Id;
            }
            _context.LabTests.Add(labTest);
            _context.SaveChanges();
            var labPrice = _context.LabTests.Include(x => x.LabTestPrice).FirstOrDefault(x => x.LabTestPrice.Id == labTest.Id);

            string body = string.Empty;

            using (StreamReader reader = new StreamReader("wwwroot/templates/labtest.html"))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{{fullname}}", labTest.Fullname);
            body = body.Replace("{{price}}", labPrice.LabTestPrice.Price.ToString("0.00"));
            body = body.Replace("{{rendezvous}}", labTest.Rendezvous.ToString("dddd, dd MMMM yyyy"));
            
            _emailService.Send(labTest.Email,"Netmeds Labtest",body);
            TempData["Success"] = "Test sorğunuz uğurlu oldu!";

            return RedirectToAction("index", "home");
        }
    }
}
