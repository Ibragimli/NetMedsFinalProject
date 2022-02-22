using Microsoft.AspNetCore.Http;
using NetMedsFull.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.Models
{
    public class Product:BaseEntity
    {
        [StringLength(maximumLength: 150)]
        public string Name { get; set; }
        public double SalePrice { get; set; }
        public double CostPrice { get; set; }
        public double DiscountPrice { get; set; }
        [StringLength(maximumLength: 30)]
        public string Country { get; set; }
        [StringLength(maximumLength: 500)]
        public string Description { get; set; }
        public bool IsTrending { get; set; }
        public bool IsNew { get; set; }
        public bool StockStatus { get; set; }
        public ProductType Type { get; set; }
        [NotMapped]
        public IFormFile ImageFiles { get; set; }

        [NotMapped]
        public IFormFile PosterImageFile { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public List<ProductFactory> ProductFactories { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
        public List<ProductBrand> ProductBrands { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
