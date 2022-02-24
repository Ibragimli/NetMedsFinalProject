using NetMedsFull.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.ViewModels
{
    public class HomeViewModel
    {
        public List<Product> NewProducts { get; set; }
        public List<Product> DiscountProducts { get; set; }
        public List<Product> FavouriteProducts { get; set; }

    }
}
