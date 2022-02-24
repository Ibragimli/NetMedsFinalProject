using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.ViewModels
{
    public class BasketViewModel
    {
        public int Count { get; set; }
        public int ProductId { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
        public double TotalPrice { get; set; }
    }
}
