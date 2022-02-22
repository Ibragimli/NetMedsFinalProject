using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.Models
{
    public class ProductCategory:BaseEntity
    {
        public int CategoryId { get; set; }
        public int ProductId { get; set; }

        public Product Product { get; set; }
        public Category Category { get; set; }
    }
}
