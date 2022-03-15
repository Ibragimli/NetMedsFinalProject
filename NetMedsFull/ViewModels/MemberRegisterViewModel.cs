using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.ViewModels
{
    public class MemberRegisterViewModel
    {
        [StringLength(maximumLength: 25)]
        [Required]
        public string Username { get; set; }
        [StringLength(maximumLength: 50)]
        [Required]
        public string Email { get; set; }
        [StringLength(maximumLength: 25)]
        [Required]
        public string Fullname { get; set; }
        [StringLength(maximumLength: 20)]
        [Required]
        public string Phone { get; set; }
        [StringLength(maximumLength: 25, MinimumLength = 8)]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [StringLength(maximumLength: 25, MinimumLength = 8)]
        [DataType(DataType.Password),Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
