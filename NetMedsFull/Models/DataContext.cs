using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.Models
{
    public class DataContext:IdentityDbContext
    {
        public DataContext(DbContextOptions options  ):base(options)
        {
        }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<TrendSlider> TrendSliders { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<SbiLab> SbiLabs { get; set; }
        public DbSet<ShopSlider> ShopSliders { get; set; }
        public DbSet<LabTest> LabTests { get; set; }
        public DbSet<SubCategoryBrand> SubCategoryBrands { get; set; }
        public DbSet<OrderSlider> OrderSliders { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Subscribe> Subscribes { get; set; }

    }
}
