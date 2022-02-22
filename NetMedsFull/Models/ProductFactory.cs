using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.Models
{
    public class ProductFactory:BaseEntity
    {
        public int ProductId { get; set; }
        public int ManufactoryId { get; set; }
        public Product Product { get; set; }
        public Manufactory Manufactory { get; set; }
    }
}
