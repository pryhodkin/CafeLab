using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CafeLab.Controllers
{
    public class MapsController : Controller
    {
        public IActionResult Cafes()
        {
            return View();
        }
    }
}
