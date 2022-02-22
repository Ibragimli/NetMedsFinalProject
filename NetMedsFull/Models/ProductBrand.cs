using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.Models
{
    public class ProductBrand : BaseEntity
    {
        public int ProductId { get; set; }
        public int BrandId { get; set; }
        public Product Product { get; set; }
        public Brand Brand { get; set; }
    }
}
