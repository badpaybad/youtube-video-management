using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MoneyNote.Identity;
using MoneyNote.Identity.Enities;
using MoneyNote.Identity.PermissionSchemes;
using MoneyNote.YoutubeManagement.Models;
using MoneyNote.YoutubeManagement.Repository;

namespace MoneyNote.YoutubeManagement.Controllers
{
    [ClaimAndValidatePermission("*", "CmsCategory")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SelectAll([FromBody] JsGridFilter filter)
        {
            return Json(new JsonResponse<List<CmsCategory>>
            {
                data = new YoutubeContentRepository().ListCategory(filter).data
            });
        }

        public IActionResult CreateOrUpdate([FromBody] CmsCategory data)
        {
            if (string.IsNullOrEmpty(data.Title)) return Json(new JsonResponse<string> { code = 1, message = "Title can not be empty" });
            if (data.Id == null || data.Id == Guid.Empty) data.Id = Guid.NewGuid();

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
            return Json(new JsonResponse<CmsCategory> { data = data });
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
            return Json(new JsonResponse<CmsCategory> { data = data });
        }

        public IActionResult GetTree()
        {
            List<CmsCategory.Dto> allCat = new List<CmsCategory.Dto>();

            using (var db = new MoneyNoteDbContext())
            {
                allCat = db.CmsCategories.Select(i => new CmsCategory.Dto
                {
                    Id = i.Id,
                    ItemsCount = i.ItemsCount,
                    ParentId = i.ParentId,
                    Title = i.Title
                }).ToList();
            }

            //var root = allCat.Where(i => i.ParentId == Guid.Empty).ToList();
            //foreach (var r in root)
            //{
            //    r.Children = allCat.Where(i => i.ParentId == r.Id).ToList();
            //    foreach (var r1 in r.Children)
            //    {
            //        r1.Children = allCat.Where(i => i.ParentId == r.Id).ToList();
            //        foreach (var r2 in r1.Children)
            //        {
            //            r2.Children = allCat.Where(i => i.ParentId == r.Id).ToList();
            //        }
            //    }
            //}

            return Json(new JsonResponse<List<CmsCategory.Dto>> { data = allCat });
        }
    }
}
