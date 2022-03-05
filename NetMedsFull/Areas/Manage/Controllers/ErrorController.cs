using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.Areas.Manage.Controllers
{
    [Area("manage")]
    public class ErrorController : Controller
    {
        public IActionResult NotFounds()
        {
            return View();
        }
    }
}
