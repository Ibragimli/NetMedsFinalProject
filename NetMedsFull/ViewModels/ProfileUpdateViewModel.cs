using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.ViewModels
{
    public class ProfileUpdateViewModel
    {
        [StringLength(maximumLength: 25)]
        public string Username { get; set; }
        [StringLength(maximumLength: 50)]
        public string Email { get; set; }
        [StringLength(maximumLength: 25)]
        public string Fullname { get; set; }
        [StringLength(maximumLength: 25)]

        public string PhoneNumber { get; set; }
        [DataType(DataType.Password)]
        [StringLength(maximumLength: 25,MinimumLength =8)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password), Compare(nameof(NewPassword))]
        [StringLength(maximumLength: 25, MinimumLength = 8)]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.Password)]
        [StringLength(maximumLength: 25, MinimumLength = 8)]
        public string CurrentPassword { get; set; }
    }
}
