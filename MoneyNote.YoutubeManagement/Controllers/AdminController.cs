using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyNote.Identity.PermissionSchemes;

namespace MoneyNote.YoutubeManagement.Controllers
{
    [ModuleDescriptor("Admin")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
