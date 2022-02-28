using NetMedsFull.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.ViewModels
{
    public class ProductShopViewModel
    {
        public List<ShopSlider> ShopSliders { get; set; }
        public List<Category> Categories { get; set; }
        public List<SubCategory> SubCategories { get; set; }
        public List<Brand> Brands { get; set; }
        public List<Product> Types { get; set; }
    }
}
