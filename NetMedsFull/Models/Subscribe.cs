using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.Models
{
    public class Subscribe : BaseEntity
    {
        [StringLength(maximumLength: 30, MinimumLength = 5)]
        public string Email { get; set; }
    }
}
