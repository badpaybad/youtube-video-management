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
                var exited = db.CmsCategories.FirstOrDefault(i => i.Id == data.Id );
                if (exited == null)
                {
                    db.CmsCategories.Add(data);
                }
                else
                {
                    exited.ParentId = data.ParentId;
                    exited.Title = data.Title;
                    exited.IsDeleted = data.IsDeleted;
                }

                db.SaveChanges();
            }
            return Json(new JsonResponse<CmsCategory> { data = data });
        }


        public IActionResult Delete([FromBody] CmsCategory data)
        {
            using (var db = new MoneyNoteDbContext())
            {
                data = db.CmsCategories.FirstOrDefault(i => i.Id == data.Id );
                if (data != null)
                {
                    db.RemoveRange(db.CmsRelations.Where(i => i.CategoryId == data.Id));
                    
                    db.CmsCategories.Remove(data);

                    db.SaveChanges();
                }
            }
            return Json(new JsonResponse<CmsCategory> { data = data });
        }

        public IActionResult GetTree()
        {
            List<CmsCategory.Dto> allCat = new List<CmsCategory.Dto>();
            int uncategoryItems;
            using (var db = new MoneyNoteDbContext())
            {
                allCat = db.CmsCategories.Select(i => new CmsCategory.Dto
                {
                    Id = i.Id,
                    ItemsCount = i.ItemsCount,
                    ParentId = i.ParentId,
                    Title = i.Title
                }).ToList();

                uncategoryItems = db.CmsContents.Where(i => db.CmsRelations.Select(r => r.ContentId).Distinct().Contains(i.Id) == false)
                    .Count();
            }         

            return Json(new JsonResponse<CategoryTree> { data = new CategoryTree { Data = allCat , UncategoryItemsCount= uncategoryItems } });
        }
    }

    public class CategoryTree
    {
        public int UncategoryItemsCount { get; set; }
        public List<CmsCategory.Dto> Data { get; set; }
    }
}
