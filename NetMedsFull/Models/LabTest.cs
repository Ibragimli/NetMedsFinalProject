using NetMedsFull.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.Models
{
    public class LabTest : BaseEntity
    {

        [Required]
        [StringLength(maximumLength: 25)]
        public string Fullname { get; set; }
        [StringLength(maximumLength: 100)]
        public string CancelComment { get; set; }
        [Required]
        [StringLength(maximumLength: 25)]
        public string Email { get; set; }
        [Required]
        public DateTime Rendezvous { get; set; }
        public int LabTestPriceId { get; set; }
        public LabTestPrice LabTestPrice { get; set; }
        public LabStatus LabStatus { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
