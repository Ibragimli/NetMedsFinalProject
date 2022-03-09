using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.Models
{
    public class TrendSlider : BaseEntity
    {
        [StringLength(maximumLength: 100)]
        public string Name { get; set; }
        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
