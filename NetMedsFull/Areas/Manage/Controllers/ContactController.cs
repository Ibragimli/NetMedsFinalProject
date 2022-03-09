using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetMedsFull.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class ContactController : Controller
    {
        private readonly DataContext _context;

        public ContactController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var contacts = _context.Contacts.ToList();
            if (contacts == null)
            {
                return RedirectToAction("notfounds", "error");
            }
            return View(contacts);
        }
        public IActionResult Edit(int id)
        {
            var contact = _context.Contacts.FirstOrDefault(x => x.Id == id);
            if (contact == null)
            {
                return RedirectToAction("notfounds", "error");
            }
            return View(contact);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Contact contact)
        {
            var existContact = _context.Contacts.FirstOrDefault(x => x.Id == contact.Id);
            if (existContact == null)
            {
                return RedirectToAction("notfounds", "error");
            }
            if (!ModelState.IsValid)
            {
                return View(existContact);
            }
            existContact.Value = contact.Value;
            _context.SaveChanges();
            TempData["Success"] = "Edit is succesfull!";
            return RedirectToAction("index", "ordersliders");
        }
    }
}
