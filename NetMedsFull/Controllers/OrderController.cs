using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetMedsFull.Models;
using NetMedsFull.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static NetMedsFull.Services.EmailServices;

namespace NetMedsFull.Controllers
{
    public class OrderController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;

        public OrderController(DataContext context, UserManager<AppUser> userManager, IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }
        public async Task<IActionResult> Checkout()
        {
            CheckoutViewModel checkoutVM = new CheckoutViewModel
            {
                CheckoutItems = await _getCheckoutItems(),
                Order = new Order(),
                OrderSliders = _context.OrderSliders.ToList(),
                MostSellingOwl = _context.Products.Include(x => x.ProductImages).Where(x => x.IsTrending).ToList(),
            };
            return View(checkoutVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order order)
        {

            AppUser user = _userManager.Users.FirstOrDefault(x => x.IsAdmin == false && x.UserName == User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction("error", "error");
            }
            List<CheckoutItemViewModel> checkoutItems = await _getCheckoutItems();

            if (checkoutItems.Count == 0)
            {
                ModelState.AddModelError("", "There is not any selected product");
            }
            if (!ModelState.IsValid)
            {
                return View("Checkout", new CheckoutViewModel { CheckoutItems = checkoutItems, Order = order });
            }

            var lastOrder = _context.Orders.OrderByDescending(x => x.Id).FirstOrDefault();
            order.CodePrefix = order.Fullname[0].ToString().ToUpper() + order.Email[0].ToString().ToUpper();
            order.CodeNumber = lastOrder == null ? 1001 : lastOrder.CodeNumber + 1;
            order.CreatedAt = DateTime.UtcNow.AddHours(4);
            order.Status = Enums.OrderStatus.Pending;
            order.DeliveryStatus = Enums.OrderDeliveryStatus.OnWaiting;
            order.OrderItems = new List<OrderItem>();
            foreach (var item in checkoutItems)
            {
                OrderItem orderItem = new OrderItem
                {
                    ProductId = item.Product.Id,
                    CostPrice = item.Product.CostPrice,
                    Count = item.Count,
                    SalePrice = item.Product.SalePrice,
                    DiscountPercent = item.Product.DiscountPercent
                };
                order.TotalAmount += orderItem.DiscountPercent > 0
                ? orderItem.SalePrice * (1 - orderItem.DiscountPercent / 100) * orderItem.Count
                : orderItem.SalePrice * orderItem.Count;
                order.OrderItems.Add(orderItem);
            }
            _context.Orders.Add(order);
            _context.BasketItems.RemoveRange(_context.BasketItems.Where(x => x.AppUserId == user.Id));
            _context.SaveChanges();

            string body = string.Empty;

            using (StreamReader reader = new StreamReader("wwwroot/templates/order.html"))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{{code}}", order.CodePrefix+order.CodeNumber);
            body = body.Replace("{{fullname}}", order.Fullname);
            body = body.Replace("{{totalAmount}}", order.TotalAmount.ToString("0.00") + "₼");
            _emailService.Send(user.Email, "Order Completed", body);
            TempData["Success"] = "Sifarisiniz uğurlu oldu!";
            return RedirectToAction("profile", "account");
        }
        private async Task<List<CheckoutItemViewModel>> _getCheckoutItems()
        {
            List<CheckoutItemViewModel> checkoutItems = new List<CheckoutItemViewModel>();

            AppUser user = null;
            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }

            if (user != null && user.IsAdmin == false)
            {
                List<BasketItem> basketItems = _context.BasketItems.Include(x => x.Product).ThenInclude(x => x.ProductImages).Where(x => x.AppUserId == user.Id).ToList();

                foreach (var item in basketItems)
                {
                    CheckoutItemViewModel checkoutItem = new CheckoutItemViewModel
                    {
                        Product = item.Product,
                        Count = item.Count
                    };
                    checkoutItems.Add(checkoutItem);
                }
            }
            else
            {
                string basketItemsStr = HttpContext.Request.Cookies["basketItemList"];
                if (basketItemsStr != null)
                {
                    List<CookieBasketItemViewModel> basketItems = JsonConvert.DeserializeObject<List<CookieBasketItemViewModel>>(basketItemsStr);

                    foreach (var item in basketItems)
                    {
                        CheckoutItemViewModel checkoutItem = new CheckoutItemViewModel
                        {
                            Product = _context.Products.Include(x => x.ProductImages).FirstOrDefault(x => x.Id == item.ProductId),
                            Count = item.Count
                        };
                        checkoutItems.Add(checkoutItem);
                    }
                }
            }

            return checkoutItems;
        }
    }
}
