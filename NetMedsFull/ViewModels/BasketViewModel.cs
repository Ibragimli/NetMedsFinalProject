using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.ViewModels
{
    public class BasketViewModel
    {
        public decimal TotalSave { get; set; }
        public decimal TotalSaveUser { get; set; }
        public decimal TotalAmount { get; set; }
        public List<BasketItemViewModel> BasketItems { get; set; }

    }
    public class BasketItemViewModel
    {
        public int Count { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public decimal SalePrice { get; set; }
        public string Name { get; set; }
        public decimal TotalPrice { get; set; }
        public bool StockStatus { get; set; }
    }
}
