using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetMedsFull.Models;
using NetMedsFull.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.Services
{
    public class LayoutService
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContext;

        public LayoutService(DataContext context, UserManager<AppUser> userManager, IHttpContextAccessor httpContext)
        {
            _context = context;
            _userManager = userManager;
            _httpContext = httpContext;
        }

        public async Task<List<Setting>> GetSettings()
        {
            return await _context.Settings.ToListAsync();
        }
        public async Task<List<Subscribe>> GetSubscribe()
        {
            return await _context.Subscribes.ToListAsync();
        }
        public async Task<List<Product>> GetProductSearch()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<BasketViewModel> GetHeaderBasket()
        {

            BasketViewModel basket = null;
            AppUser user = null;
            if (_httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(_httpContext.HttpContext.User.Identity.Name);
            }
            if (user != null && user.IsAdmin == false)
            {
                basket = _getBasketItems(_context.BasketItems.Include(x => x.Product).Where(x => x.AppUserId == user.Id).ToList());
            }
            else
            {
                var basketItemStr = _httpContext.HttpContext.Request.Cookies["basketItemList"];
                if (basketItemStr != null)
                {
                    List<CookieBasketItemViewModel> cookieItems = JsonConvert.DeserializeObject<List<CookieBasketItemViewModel>>(basketItemStr);
                    basket = _getBasketItems(cookieItems);
                }
            }
            return basket; 
        }
        private BasketViewModel _getBasketItems(List<CookieBasketItemViewModel> cookieBasketItems)
        {
            BasketViewModel basket = new BasketViewModel
            {
                BasketItems = new List<BasketItemViewModel>(),
            };
            foreach (var item in cookieBasketItems)
            {
                Product product = _context.Products.FirstOrDefault(x => x.Id == item.ProductId);
                BasketItemViewModel basketItem = new BasketItemViewModel
                {
                    Name = product.Name,
                    Price = (product.DiscountPercent > 0 ? (product.SalePrice * (1 - product.DiscountPercent / 100)) : product.SalePrice),
                    SalePrice = product.SalePrice,
                    ProductId = product.Id,
                    Count = item.Count,
                };
                basketItem.TotalPrice = basketItem.Count * basketItem.Price;
                basket.TotalAmount += basketItem.TotalPrice;
                basket.TotalSave += (basketItem.Count * basketItem.SalePrice) - (basketItem.Count * basketItem.Price);
                basket.BasketItems.Add(basketItem);

            }
            return basket;
        }

        private BasketViewModel _getBasketItems(List<BasketItem> basketItems)
        {
            BasketViewModel basket = new BasketViewModel
            {
                BasketItems = new List<BasketItemViewModel>(),
            };
            foreach (var item in basketItems)
            {
                BasketItemViewModel basketItem = new BasketItemViewModel
                {
                    Name = item.Product.Name,
                    Price = item.Product.DiscountPercent > 0 ? (item.Product.SalePrice * (1 - item.Product.DiscountPercent / 100)) : item.Product.SalePrice,
                    ProductId = item.Product.Id,
                    Count = item.Count,
                    SalePrice = item.Product.SalePrice,
                };
                basketItem.TotalPrice = basketItem.Count * basketItem.Price;
                basket.TotalAmount += basketItem.TotalPrice;
                basket.TotalSave += (basketItem.Count * basketItem.SalePrice) - (basketItem.Count * basketItem.Price);
                basket.BasketItems.Add(basketItem);
            }

            return basket;
        }
    }
}
