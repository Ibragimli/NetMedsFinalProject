using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.Models
{
    public class Comment:BaseEntity
    {
        public int ProductId { get; set; }
        public int AppUserId { get; set; }
        [StringLength(maximumLength: 50)]
        public string Title { get; set; }
        [StringLength(maximumLength: 25)]
        public string Fullname { get; set; }
        public bool CommentStatus { get; set; }
        [StringLength(maximumLength: 50)]
        public string Email { get; set; }
        [StringLength(maximumLength: 500)]
        public string Text { get; set; }
        [Required]
        [Range(1,5)]
        public int Rate { get; set; }
        public DateTime Time { get; set; }
        public Product Product { get; set; }
        public AppUser AppUser { get; set; }
    }
}
