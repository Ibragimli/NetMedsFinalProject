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
    public class LabtestController : Controller
    {
        private readonly DataContext _context;

        public LabtestController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var labtests = _context.LabTests.Include(x => x.LabTestPrice).ToList();
            if (labtests == null)
            {
                return RedirectToAction("notfounds", "error");
            }
            return View(labtests);
        }

        public IActionResult AcceptLab(int id)
        {
            var labtestExist = _context.LabTests.FirstOrDefault(x => x.Id == id);
            if (labtestExist == null)
            {
                return RedirectToAction("notfounds", "error");
            }
            labtestExist.LabStatus = Enums.LabStatus.Accepted;
            _context.SaveChanges();

            return RedirectToAction("index", "labtest");
        }
        [HttpPost]
        public IActionResult CancelLab(LabTest labTest)
        {
            var labtestExist = _context.LabTests.FirstOrDefault(x => x.Id == labTest.Id);
            if (labtestExist == null)
            {
                return RedirectToAction("notfounds", "error");
            }

            if (labTest.CancelComment == null /*labTest.CancelComment.Length < 0 */)
            {
                ModelState.AddModelError("cancelcomment", "Cancel Comment is required!");
                TempData["error"] = "Cancel Comment is required!";
                return RedirectToAction("index","labtest");
            }

            labtestExist.LabStatus = Enums.LabStatus.Canceled;
            labtestExist.CancelComment = labTest.CancelComment;
            _context.SaveChanges();
            return RedirectToAction("index", "labtest");
        }
    }
}
