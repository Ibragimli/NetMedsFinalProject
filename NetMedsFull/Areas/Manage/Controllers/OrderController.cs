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
    public class OrderController : Controller
    {

        private readonly DataContext _context;

        public OrderController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var orders = _context.Orders.Include(x => x.OrderItems).ToList();
            if (orders == null)
            {
                return RedirectToAction("notfounds", "error");
            }
            return View(orders);
        }

        public IActionResult AcceptOrder(int id)
        {
            var orderExist = _context.Orders.FirstOrDefault(x => x.Id == id);
            if (orderExist == null)
            {
                return RedirectToAction("notfounds", "error");
            }
            orderExist.Status = Enums.OrderStatus.Accepted;
            _context.SaveChanges();

            return RedirectToAction("index", "order");
        }
        [HttpPost]
        public IActionResult RejectedOrder(Order Order)
        {
            var orderExist = _context.Orders.FirstOrDefault(x => x.Id == Order.Id);
            if (orderExist == null)
            {
                return RedirectToAction("notfounds", "error");
            }

            if (Order.RejectComment == null)
            {
                ModelState.AddModelError("rejectcomment", "Reject comment is required!");
                TempData["error"] = "Reject Comment is required!";
                return RedirectToAction("index", "order");
            }

            orderExist.Status = Enums.OrderStatus.Rejected;
            orderExist.RejectComment = Order.RejectComment;
            _context.SaveChanges();
            return RedirectToAction("index", "order");
        }
    }
}
