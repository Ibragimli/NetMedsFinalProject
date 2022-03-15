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
    public class DashboardController : Controller
    {
        private readonly DataContext _context;

        public DashboardController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            var orders = _context.Orders.Where(x=>x.Status == Enums.OrderStatus.Accepted).ToList();
            var monthOrders = _context.Orders.Where(x => x.Status == Enums.OrderStatus.Accepted).Where(x => x.CreatedAt.Month == DateTime.Now.Month).ToList();
            var user = _context.Users.Where(x => x.IsAdmin == false).ToList();
            decimal total = 0;
            decimal totalMonth = 0;
            int productsCount = 0;
            foreach (var item in orders)
            {
                total += item.TotalAmount;
            }
            foreach (var item in monthOrders)
            {
                totalMonth += item.TotalAmount;
            }
            foreach (var item in products)
            {
                productsCount++;
            }


            ViewBag.TotalAmountSell = total;
            ViewBag.MonthTotalAmountSell = totalMonth;
            ViewBag.UserCount = user.Count();
            ViewBag.ProductsCount = productsCount;
            return View();
        }
    }
}
