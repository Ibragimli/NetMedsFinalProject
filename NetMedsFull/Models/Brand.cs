using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.Models
{
    public class Brand : BaseEntity
    {
        [StringLength(maximumLength: 25)]
        public string Name { get; set; }
        
        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }
        public List<Product> Products { get; set; }
    }
}
