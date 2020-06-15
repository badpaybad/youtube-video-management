using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using MoneyNote.Identity.Enities;
using MoneyNote.Identity.PermissionSchemes;
using MoneyNote.YoutubeManagement.Models;

namespace MoneyNote.YoutubeManagement.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route(Auth.LoginUrl)]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route(Auth.LoginUrl)]
        public IActionResult Login([FromBody] User user)
        {
            var ok = Auth.Login(user.Username, user.Password, HttpContext, out string token);

            return Json(new AjaxResponse<string>
            {
                code = ok == true ? 0: 1,
                message = ok == true ? "Loged" : "Wrong username or password",
                data = token
            });
        }
        [Route(Auth.LogoutUrl)]
        public IActionResult Logout()
        {
            Auth.Logout(HttpContext);

            return Redirect("/");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
