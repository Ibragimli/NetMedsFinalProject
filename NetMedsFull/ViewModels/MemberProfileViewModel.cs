using NetMedsFull.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.ViewModels
{
    public class MemberProfileViewModel
    {
        public ProfileUpdateViewModel ProfileUpdateViewModel { get; set; }
        public List<Order> Orders { get; set; }
        public List<LabTest> LabTests { get; set; }

    }
}
