using NetMedsFull.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.ViewModels
{
    public class LabTestViewModel
    {
        public LabTest Labtest { get; set; }
        public List<LabTestPrice> LabTestPrice { get; set; }
    }
}
