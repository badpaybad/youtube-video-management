﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MoneyNote.Identity;
using MoneyNote.Identity.Enities;
using MoneyNote.Identity.PermissionSchemes;
using MoneyNote.YoutubeManagement.Models;

namespace MoneyNote.YoutubeManagement.Controllers
{
    [ClaimAndValidatePermission("*", "CmsCategory")]
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateOrUpdate([FromBody] CmsCategory data)
        {
            if (string.IsNullOrEmpty(data.Title)) return Json(new AjaxResponse<string> { code = 1, message = "Title can not be empty" });
            if (data.Id == null || data.Id== Guid.Empty) data.Id = Guid.NewGuid();

            using (var db = new MoneyNoteDbContext())
            {
                var exited = db.CmsCategories.FirstOrDefault(i => i.Id == data.Id && i.IsDeleted == 0);
                if (exited == null)
                {
                    db.CmsCategories.Add(data);
                }
                else
                {
                    exited.ParentId = data.ParentId;
                    exited.Title = data.Title;
                }

                db.SaveChanges();
            }
            return Json(new AjaxResponse<CmsCategory> { data = data });
        }

        public IActionResult SelectAll([FromBody] JsGridFilter filter)
        {
            using (var db = new MoneyNoteDbContext())
            {
                return Json(new AjaxResponse<List<CmsCategory>>
                {
                    data = db.CmsCategories.Where(i => i.IsDeleted == 0).ToList()
                });
            }
        }

        public IActionResult Delete([FromBody] CmsCategory data)
        {
            using (var db = new MoneyNoteDbContext())
            {
                data = db.CmsCategories.FirstOrDefault(i => i.Id == data.Id && i.IsDeleted == 0);
                if (data != null)
                {
                    data.IsDeleted = 1;
                    db.SaveChanges();
                }
            }
            return Json(new AjaxResponse<CmsCategory> { data = data });
        }
    }
}