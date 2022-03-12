using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.ViewModels
{
    public class WishViewModel
    {
        public List<WishItemViewModel> WishItems { get; set; }

    }
    public class WishItemViewModel
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public decimal SalePrice { get; set; }
        public decimal DiscountPercent { get; set; }
        public string Name { get; set; }
        public bool StockStatus { get; set; }
    }
}
