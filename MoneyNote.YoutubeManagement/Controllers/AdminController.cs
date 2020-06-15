using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyNote.Identity.Enities;
using MoneyNote.Identity.PermissionSchemes;

namespace MoneyNote.YoutubeManagement.Controllers
{
    [ClaimAndValidatePermission("*", "AdminDashboard")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


    }
}
