using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.Models
{
    public class AppUser:IdentityUser
    {
        [StringLength(maximumLength: 50)]
        public string FullName { get; set; }
        public bool IsAdmin { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Order> Orders { get; set; }
        public List<LabTest> LabTests { get; set; }

    }
}
