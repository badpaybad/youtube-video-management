﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MoneyNote.Identity.PermissionSchemes;

namespace MoneyNote.YoutubeManagement.Controllers
{
    [ClaimAndValidatePermission("*","CmsContent")]
    public class ContentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}