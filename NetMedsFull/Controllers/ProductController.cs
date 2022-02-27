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
    public class ProductController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ProductController(DataContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Detail(int id)
        {
            Product product = _getProductContext(id);
            if (product == null)
            {
                return RedirectToAction("error", "error");

            }
            ProductDetailViewModel productDetailVM = GetProductDetail(product, new Comment());
            return View(productDetailVM);
        }

        [HttpPost]
        public async Task<IActionResult> Comment(Comment comment)
        {
            Product product = _getProductContext(comment.ProductId);
            if (product == null)
            {
                return RedirectToAction("error", "error");
            }
            ProductDetailViewModel productDetailVM = GetProductDetail(product, comment);

            if (!ModelState.IsValid)
            {
                return View("Detail", productDetailVM);
            }
            if (!_context.Products.Any(x => x.Id == comment.ProductId))
            {
                return View("Detail", productDetailVM);
            }

            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            comment.Email = user.Email;
            comment.Fullname = user.FullName;
            comment.AppUserId = user.Id;
            comment.CommentStatus = false;
            comment.Time = DateTime.UtcNow.AddHours(4);
            _context.Comments.Add(comment);
            _context.SaveChanges();
            return RedirectToAction("detail", new { Id = comment.ProductId });
        }


        //[HttpPost]
        //public IActionResult Comment(Comment comment)
        //{
        //    Comment comment1 = comment;

        //    return Ok(comment1);
        //}

        public IActionResult Shop()
        {
            return View();
        }

        public async Task<IActionResult> AddBasket(int id)
        {
            if (!_context.Products.Any(x => x.Id == id))
            {
                return RedirectToAction("error", "error");
            }
            BasketViewModel basketData = null;
            AppUser user = null;
            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }

            if (user != null && user.IsAdmin == false)
            {


                BasketItem basketItem = _context.BasketItems.FirstOrDefault(x => x.AppUserId == user.Id && x.ProductId == id);

                if (basketItem == null)
                {
                    basketItem = new BasketItem
                    {
                        Count = 1,
                        AppUserId = user.Id,
                        ProductId = id,
                    };
                    _context.BasketItems.Add(basketItem);
                }
                else
                {
                    basketItem.Count++;
                }
                _context.SaveChanges();

                basketData = _getBasketItems(_context.BasketItems.Include(x => x.Product).Where(x => x.AppUserId == user.Id).ToList());

            }
            else
            {
                List<CookieBasketItemViewModel> basketItems = new List<CookieBasketItemViewModel>();
                string existBasketItem = HttpContext.Request.Cookies["basketItemList"];
                if (existBasketItem != null)
                {
                    basketItems = JsonConvert.DeserializeObject<List<CookieBasketItemViewModel>>(existBasketItem);
                }
                CookieBasketItemViewModel item = basketItems.FirstOrDefault(x => x.ProductId == id);
                if (item == null)
                {
                    item = new CookieBasketItemViewModel
                    {
                        ProductId = id,
                        Count = 1,
                    };
                    basketItems.Add(item);
                }
                else
                {
                    item.Count++;
                }
                var productIdStr = JsonConvert.SerializeObject(basketItems);
                HttpContext.Response.Cookies.Append("basketItemList", productIdStr);
                basketData = _getBasketItems(basketItems);
            }
            return Ok(basketData);
        }
        public ActionResult ShowBasket()
        {
            var productIdStr = HttpContext.Request.Cookies["basketItemList"];
            List<CookieBasketItemViewModel> basketItem = new List<CookieBasketItemViewModel>();

            if (productIdStr != null)
            {
                basketItem = JsonConvert.DeserializeObject<List<CookieBasketItemViewModel>>(productIdStr);
            }

            return Json(basketItem);
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
                    StockStatus = product.StockStatus,
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
                    StockStatus = item.Product.StockStatus,


                };
                basketItem.TotalPrice = basketItem.Count * basketItem.Price;
                basket.TotalAmount += basketItem.TotalPrice;
                basket.TotalSaveUser += (basketItem.Count * basketItem.SalePrice) - (basketItem.Count * basketItem.Price);
                basket.BasketItems.Add(basketItem);
            }


            return basket;
        }


        private Product _getProductContext(int id)
        {

            Product product = _context.Products.Include(x => x.ProductImages)
                .Include(x => x.Brand).ThenInclude(x => x.SubCategory)
                .Include(x => x.Comments)
                .FirstOrDefault(x => x.Id == id);
            return product;
        }

        private ProductDetailViewModel GetProductDetail(Product product, Comment comment = null)
        {

            ProductDetailViewModel productDetailVM = new ProductDetailViewModel
            {
                Products = product,
                Comments = comment,
                RelatedProduct = _context.Products
                .Include(x => x.ProductImages).Include(x => x.Brand)
                .Where(x => x.Brand.Id == product.BrandId).OrderByDescending(x => x.Id).Take(5).ToList()
            };
            return productDetailVM;
        }
    }
}
