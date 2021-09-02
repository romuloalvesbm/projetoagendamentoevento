using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Projeto.Presentation.Areas.AreaRestrita.Controllers
{
    [Authorize]
    [Area("AreaRestrita")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}