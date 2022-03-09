using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.Models
{
    public class Contact : BaseEntity
    {
        [StringLength(maximumLength: 50)]
        public string Key { get; set; }
        [StringLength(maximumLength: 1000)]
        public string Value { get; set; }
    }
}
