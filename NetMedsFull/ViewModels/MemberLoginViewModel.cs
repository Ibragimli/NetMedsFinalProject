using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.ViewModels
{
    public class MemberLoginViewModel
    {
        [StringLength(maximumLength:25)]
        public string Username { get; set; }
        [StringLength(maximumLength: 25,MinimumLength =8)]
        public string Password { get; set; }
    }
}
