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
            var orders = _context.Orders.ToList();
            var monthOrders = _context.Orders.Where(x=>x.CreatedAt.Month == DateTime.Now.Month).ToList();
            var user = _context.Users.Where(x=>x.IsAdmin == false).ToList();
            decimal total = 0;
            decimal totalMonth = 0;
            foreach (var item in orders)
            {
                total += item.TotalAmount;
            }
            foreach (var item in monthOrders)
            {
                totalMonth += item.TotalAmount;
            }

            ViewBag.TotalAmountSell = total;
            ViewBag.MonthTotalAmountSell = totalMonth;
            ViewBag.UserCount = user.Count();
            return View();
        }
    }
}
