using NetMedsFull.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.ViewModels
{
    public class ProductDetailViewModel
    {
        public Comment Comments { get; set; }
        public List<Product> RelatedProduct { get; set; }
        public Product Products { get; set; }
    }
}
