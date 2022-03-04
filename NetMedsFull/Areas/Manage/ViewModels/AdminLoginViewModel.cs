using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.Areas.Manage.ViewModels
{
    public class AdminLoginViewModel
    {
        [StringLength(maximumLength: 25)]
        public string Username { get; set; }
        [DataType(DataType.Password)]
        [StringLength(maximumLength: 25, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
