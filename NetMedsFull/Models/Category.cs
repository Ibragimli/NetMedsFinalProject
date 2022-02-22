using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.Models
{
    public class Category:BaseEntity
    {
        [StringLength(maximumLength: 25)]
        public string Name { get; set; }
        public List<SubCategory> SubCategories { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
    }
}
