using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetMedsFull.Enums;
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
        public IActionResult RejectOrder(Order Order)
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

        public IActionResult StatusDelivered(int id)
        {
            var orderExist = _context.Orders.FirstOrDefault(x => x.Id == id);
            if (orderExist == null)
            {
                return RedirectToAction("notfounds", "error");
            }
            orderExist.DeliveryStatus = Enums.OrderDeliveryStatus.Delivered;
            _context.SaveChanges();
            return RedirectToAction("view", new { Id = orderExist.Id });
        }
        public IActionResult StatusOnProcessing(int id)
        {
            var orderExist = _context.Orders.FirstOrDefault(x => x.Id == id);
            if (orderExist == null)
            {
                return RedirectToAction("notfounds", "error");
            }
            orderExist.DeliveryStatus = Enums.OrderDeliveryStatus.OnProcessing;
            _context.SaveChanges();
            return RedirectToAction("view", new { Id = orderExist.Id });
        }

        public IActionResult StatusOnCourier(int id)
        {
            var orderExist = _context.Orders.FirstOrDefault(x => x.Id == id);
            if (orderExist == null)
            {
                return RedirectToAction("notfounds", "error");
            }
            orderExist.DeliveryStatus = Enums.OrderDeliveryStatus.OnCourier;
            _context.SaveChanges();
            return RedirectToAction("view", new { Id = orderExist.Id });
        }
        public IActionResult StatusOnDepot(int id)
        {
            var orderExist = _context.Orders.FirstOrDefault(x => x.Id == id);
            if (orderExist == null)
            {
                return RedirectToAction("notfounds", "error");
            }
            orderExist.DeliveryStatus = Enums.OrderDeliveryStatus.OnDepot;
            _context.SaveChanges();
            return RedirectToAction("view", new { Id = orderExist.Id });
        }
        public IActionResult StatusOnWaiting(int id)
        {
            var orderExist = _context.Orders.FirstOrDefault(x => x.Id == id);
            if (orderExist == null)
            {
                return RedirectToAction("notfounds", "error");
            }
            orderExist.DeliveryStatus = Enums.OrderDeliveryStatus.OnWaiting;
            _context.SaveChanges();
            return RedirectToAction("view", new { Id = orderExist.Id });
        }
        public IActionResult View(int id)
        {
            var order = _context.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Product).FirstOrDefault(x => x.Id == id);
            if (order == null)
            {
                return RedirectToAction("notfounds", "error");
            }
            return View(order);
        }
    }
}
