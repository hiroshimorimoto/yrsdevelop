using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace YrsWeb.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {

        [HttpGet("{loginType}")]
        public IActionResult Index(string loginType)
        {
            string viewName = String.Format("/Views/{0}/login.html", loginType);
            return View(viewName);
        }
    }
}
