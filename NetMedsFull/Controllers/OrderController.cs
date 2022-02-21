using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Cart()
        {
            return View();
        }
    }
}
