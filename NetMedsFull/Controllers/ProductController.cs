using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetMedsFull.Enums;
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
            ViewBag.productId = id;
            ViewBag.RatePoint = 0;
            int ratePoint = 0;
            int countRate = 0;
            Product product = _getProductContext(id);
            if (product == null)
            {
                return RedirectToAction("error", "error");
            }

            if (product.Comments.Count() > 0)
            {
                foreach (var comment in product.Comments)
                {
                    countRate++;
                    ratePoint += comment.Rate;
                }
                ratePoint = ratePoint / countRate;
                ViewBag.RatePoint = ratePoint;
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
                return RedirectToAction("Error", "error");
            }
            ProductDetailViewModel productDetailVM = GetProductDetail(product, comment);

            if (!ModelState.IsValid)
            {
                TempData["error"] = "Comment model is not valid!";
                return View("Detail", productDetailVM);
            }
            if (!_context.Products.Any(x => x.Id == comment.ProductId))
            {
                TempData["error"] = "Selected Product not found";
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
            TempData["Success"] = "Comment added successfully";
            return RedirectToAction("detail", new { Id = comment.ProductId });
        }



        public IActionResult Shop( int? categoryId = null, int? subcategoryId = null, int? brandId = null, int? maxPrice = null, int? minPrice = null, ProductType? typeId = null, int page = 1)
        {
            var products = _context.Products.Include(x => x.Brand).ThenInclude(x => x.SubCategoryBrands).Include(x => x.ProductImages).Where(x => x.IsDelete == false).AsQueryable();
            ViewBag.BrandId = brandId;
            ViewBag.CategoryId = categoryId;
            ViewBag.SubCategoryId = subcategoryId;
            ViewBag.TypeId = typeId;
            ViewBag.PageIndex = page;
            ViewBag.MinPrice = (int)products.Min(x => x.SalePrice);
            ViewBag.MaxPrice = (int)products.Max(x => x.SalePrice);

            if (minPrice != null && maxPrice != null)
                products = products.Where(x => x.SalePrice >= minPrice && x.SalePrice <= maxPrice);

            ViewBag.SelectedMinPrice = minPrice ?? ViewBag.MinPrice;
            ViewBag.SelectedMaxPrice = maxPrice ?? ViewBag.MaxPrice;

            if (brandId != null)
            {
                products = products.Where(x => x.Brand.Id == brandId);
            }

            if (subcategoryId != null)
            {
                products = products.Where(x => x.Brand.SubCategoryBrands.Any(x => x.SubCategory.Id == subcategoryId));
            }
            if (categoryId != null)
            {
                products = products.Where(x => x.Brand.SubCategoryBrands.Any(x => x.SubCategory.CategoryId == categoryId));
            }
            if (minPrice != null)
            {
                products = products.Where(x => x.SalePrice > minPrice);
            }
            if (maxPrice != null)
            {
                products = products.Where(x => x.SalePrice < maxPrice);
            }
            if (typeId != null)
            {
                products = products.Where(x => x.Type == typeId.Value);
            }


            ProductShopViewModel productDetailVM = new ProductShopViewModel
            {
                ShopSliders = _context.ShopSliders.ToList(),
                Categories = _context.Categories.Where(x => x.IsDelete == false).ToList(),
                Brands = _context.Brands.Include(x => x.SubCategoryBrands).ThenInclude(x => x.SubCategory).Where(x => x.IsDelete == false).ToList(),
                SubCategories = _context.SubCategories.Where(x => x.IsDelete == false).ToList(),
                PagenatedProducts = PagenetedList<Product>.Create(products, page, 5)
            };
            return View(productDetailVM);
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

        public async Task<IActionResult> DeleteBasket(int id)
        {
            if (!_context.Products.Any(x => x.Id == id))
            {
                return RedirectToAction("error", "error");
            }
            List<BasketItemViewModel> productsDetail = new List<BasketItemViewModel>();

            if (User.Identity.IsAuthenticated)
            {
                BasketItem basketItem = _context.BasketItems.FirstOrDefault(x => x.ProductId == id);
                if (basketItem == null)
                {
                    return RedirectToAction("error", "error");
                }
                if (basketItem.Count == 1)
                {
                    _context.BasketItems.Remove(basketItem);
                }
                else
                {
                    basketItem.Count--;
                }
                _context.SaveChanges();
            }
            else
            {
                string basket = HttpContext.Request.Cookies["basketItemList"];
                productsDetail = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basket);
                BasketItemViewModel productBasket = productsDetail.FirstOrDefault(x => x.ProductId == id);
                if (productBasket == null)
                {
                    return RedirectToAction("error", "error");
                }
                if (productBasket.Count == 1)
                {
                    productsDetail.Remove(productBasket);
                }
                else
                {
                    productBasket.Count--;
                }
                HttpContext.Response.Cookies.Append("basketItemList", JsonConvert.SerializeObject(productsDetail));
            }
            return Ok(productsDetail);

        }

        //public ActionResult ShowBasket()
        //{
        //    var productIdStr = HttpContext.Request.Cookies["basketItemList"];
        //    List<CookieBasketItemViewModel> basketItem = new List<CookieBasketItemViewModel>();

        //    if (productIdStr != null)
        //    {
        //        basketItem = JsonConvert.DeserializeObject<List<CookieBasketItemViewModel>>(productIdStr);
        //    }

        //    return Json(basketItem);
        //}


        //public ActionResult ShowWish()
        //{
        //    var productIdStr = HttpContext.Request.Cookies["wishItemList"];
        //    List<CookieWishItemViewModel> wishItem = new List<CookieWishItemViewModel>();

        //    if (productIdStr != null)
        //    {
        //        wishItem = JsonConvert.DeserializeObject<List<CookieWishItemViewModel>>(productIdStr);
        //    }

        //    return Json(wishItem);
        //}

        public async Task<IActionResult> AddWish(int id)
        {
            if (!_context.Products.Any(x => x.Id == id))
            {
                return RedirectToAction("error", "error");
            }
            WishViewModel wishData = null;
            AppUser user = null;
            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }

            if (user != null && user.IsAdmin == false)
            {

                WishItem wishItem = _context.WishItems.FirstOrDefault(x => x.AppUserId == user.Id && x.ProductId == id);

                if (wishItem == null)
                {
                    wishItem = new WishItem
                    {
                        AppUserId = user.Id,
                        ProductId = id,
                    };
                    _context.WishItems.Add(wishItem);
                }

                _context.SaveChanges();

                wishData = _getWishItems(_context.WishItems.Include(x => x.Product).Where(x => x.AppUserId == user.Id).ToList());

            }
            else
            {
                List<CookieWishItemViewModel> wishItems = new List<CookieWishItemViewModel>();
                string existWishItem = HttpContext.Request.Cookies["wishItemList"];
                if (existWishItem != null)
                {
                    wishItems = JsonConvert.DeserializeObject<List<CookieWishItemViewModel>>(existWishItem);
                }
                CookieWishItemViewModel item = wishItems.FirstOrDefault(x => x.ProductId == id);
                if (item == null)
                {
                    item = new CookieWishItemViewModel
                    {
                        ProductId = id,
                    };
                    wishItems.Add(item);
                }

                var productIdStr = JsonConvert.SerializeObject(wishItems);
                HttpContext.Response.Cookies.Append("wishItemList", productIdStr);
                wishData = _getWishItems(wishItems);
            }
            return Ok(wishData);
        }

        public async Task<IActionResult> DeleteWish(int id)
        {
            if (!_context.Products.Any(x => x.Id == id))
            {
                return RedirectToAction("error", "error");
            }
            List<WishItemViewModel> wishItems = new List<WishItemViewModel>();

            if (User.Identity.IsAuthenticated)
            {
                WishItem wishItem = _context.WishItems.FirstOrDefault(x => x.ProductId == id);
                if (wishItem == null)
                {
                    return RedirectToAction("error", "error");
                }

                _context.WishItems.Remove(wishItem);
                _context.SaveChanges();
            }
            else
            {
                string wish = HttpContext.Request.Cookies["wishItemList"];
                wishItems = JsonConvert.DeserializeObject<List<WishItemViewModel>>(wish);
                WishItemViewModel productWish = wishItems.FirstOrDefault(x => x.ProductId == id);
                if (productWish == null)
                {
                    return RedirectToAction("error", "error");
                }

                wishItems.Remove(productWish);

                HttpContext.Response.Cookies.Append("wishItemList", JsonConvert.SerializeObject(wishItems));
            }
            return Ok(wishItems);

        }

        private WishViewModel _getWishItems(List<CookieWishItemViewModel> cookieWishItems)
        {

            WishViewModel wishItems = new WishViewModel()
            {
                WishItems = new List<WishItemViewModel>(),
            };
            foreach (var item in cookieWishItems)
            {
                Product product = _context.Products.FirstOrDefault(x => x.Id == item.ProductId);
                WishItemViewModel wishItem = new WishItemViewModel
                {
                    Name = product.Name,
                    Price = (product.DiscountPercent > 0 ? (product.SalePrice * (1 - product.DiscountPercent / 100)) : product.SalePrice),
                    SalePrice = product.SalePrice,
                    ProductId = product.Id,
                    StockStatus = product.StockStatus,
                    DiscountPercent = product.DiscountPercent,
                };

            }
            return wishItems;

        }
        private WishViewModel _getWishItems(List<WishItem> wishItems)
        {
            WishViewModel wish = new WishViewModel
            {
                WishItems = new List<WishItemViewModel>(),
            };
            foreach (var item in wishItems)
            {
                WishItemViewModel wishItem = new WishItemViewModel
                {
                    Name = item.Product.Name,
                    Price = item.Product.DiscountPercent > 0 ? (item.Product.SalePrice * (1 - item.Product.DiscountPercent / 100)) : item.Product.SalePrice,
                    ProductId = item.Product.Id,
                    StockStatus = item.Product.StockStatus,
                };
                wish.WishItems.Add(wishItem);
            }
            return wish;
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
                    SalePrice = item.Product.SalePrice,
                };
                basketItem.TotalPrice = basketItem.Count * basketItem.Price;
                basket.TotalAmount += basketItem.TotalPrice;
                basket.TotalSaveUser += ((basketItem.Count * basketItem.SalePrice) - (basketItem.Count * basketItem.Price));
                basket.BasketItems.Add(basketItem);
            }


            return basket;
        }
        private Product _getProductContext(int id)
        {

            Product product = _context.Products.Include(x => x.ProductImages)
                .Include(x => x.Brand).ThenInclude(x => x.SubCategoryBrands)
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
                .Where(x => x.Brand.Id == product.BrandId).OrderByDescending(x => x.Id).Take(5).ToList(),
            };
            return productDetailVM;
        }






    }
}
