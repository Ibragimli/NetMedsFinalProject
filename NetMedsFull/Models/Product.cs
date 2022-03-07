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
    public class Product : BaseEntity
    {
        [StringLength(maximumLength: 150)]
        public string Name { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalePrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Range(1, 100)]
        public decimal DiscountPercent { get; set; }
        [StringLength(maximumLength: 30)]
        public string Country { get; set; }
        [StringLength(maximumLength: 500)]
        public string Description { get; set; }
        public bool IsTrending { get; set; }
        public bool IsNew { get; set; }
        public bool StockStatus { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public ProductType Type { get; set; }
        [NotMapped]
        public List<IFormFile> ImageFiles { get; set; }
        [NotMapped]
        public IFormFile PosterImageFile { get; set; }
        [NotMapped]
        public List<int> ProductImagesIds { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public List<Comment> Comments { get; set; }
        public List<TrendSlider> TrendSliders { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
