using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.ViewModels
{
    public class LabTestPostViewModel
    {

        public string Fullname { get; set; }
        public string Email { get; set; }
        public DateTime Rendezvous { get; set; }
        public int LabTestPriceId { get; set; }
    }
}
