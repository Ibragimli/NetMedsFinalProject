using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.Models
{
    public class SubCategoryBrand : BaseEntity
    {
        public int BrandId { get; set; }
        public int SubCategoryId { get; set; }

        public SubCategory SubCategory { get; set; }
        public Brand Brand { get; set; }
    }
}
