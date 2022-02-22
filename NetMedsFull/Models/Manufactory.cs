using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.Models
{
    public class Manufactory:BaseEntity
    {
        [StringLength(maximumLength:25)]
        public string Name { get; set; }
        public List<ProductFactory> ProductFactories { get; set; }

    }
}
