using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetMedsFull.Models;
using NetMedsFull.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.Controllers
{
    public class WishController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;

        public WishController(DataContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> WishList()
        {
            WishListViewModel wishListVM = new WishListViewModel
            {
                WishlistItems = await _getWishItems(),
            };
            return View(wishListVM);

        }

        private async Task<List<WishlistItemViewModel>> _getWishItems()
        {
            List<WishlistItemViewModel> wishlistItems = new List<WishlistItemViewModel>();

            AppUser user = null;
            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }

            if (user != null && user.IsAdmin == false)
            {
                List<WishItem> wishItems = _context.WishItems.Include(x => x.Product).ThenInclude(x => x.ProductImages).Where(x => x.AppUserId == user.Id).ToList();

                foreach (var item in wishItems)
                {
                    WishlistItemViewModel wishlistItem = new WishlistItemViewModel
                    {
                        Product = item.Product,

                    };
                    wishlistItems.Add(wishlistItem);
                }
            }
            else
            {
                string wishItemsStr = HttpContext.Request.Cookies["wishItemList"];
                if (wishItemsStr != null)
                {
                    List<CookieWishItemViewModel> cookieWishItems = JsonConvert.DeserializeObject<List<CookieWishItemViewModel>>(wishItemsStr);

                    foreach (var item in cookieWishItems)
                    {
                        WishlistItemViewModel checkoutItem = new WishlistItemViewModel
                        {
                            Product = _context.Products.Include(x => x.ProductImages).FirstOrDefault(x => x.Id == item.ProductId),

                        };
                        wishlistItems.Add(checkoutItem);
                    }
                }
            }

            return wishlistItems;
        }
    }
}
