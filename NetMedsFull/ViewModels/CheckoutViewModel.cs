using NetMedsFull.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.ViewModels
{
    public class CheckoutViewModel
    {
        public List<CheckoutItemViewModel> CheckoutItems { get; set; }
        public Order Order { get; set; }
        public List<OrderSlider> OrderSliders { get; set; }
        public List<Product> MostSellingOwl { get; set; }
    }
}
