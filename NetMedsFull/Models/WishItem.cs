using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.Models
{
    public class WishItem:BaseEntity
    {
        public string AppUserId { get; set; }
        public int ProductId { get; set; }
        public AppUser AppUser { get; set; }
        public Product Product { get; set; }
    }
}
