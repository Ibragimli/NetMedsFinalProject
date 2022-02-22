using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.ViewModels
{
    public class ForgotViewModel
    {
        [Required]
        [StringLength(maximumLength:50)]
        public string Email { get; set; }
    }
}
