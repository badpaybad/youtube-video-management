using System;
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
        //[ApiExplorerSettings(GroupName = "Category")]
        public IActionResult SelectAll([FromBody] JsGridFilter filter)
        {
            filter = filter ?? new JsGridFilter();
            filter.categoryIds = filter.categoryIds ?? new List<Guid>();
            filter.categoryIds.Where(i => i != null && i != Guid.Empty).ToList();

            using (var db = new MoneyNoteDbContext())
            {
                var query = db.CmsCategories.Where(i => i.IsDeleted == 0);
                if (!string.IsNullOrEmpty(filter.title))
                {
                    query = query.Where(i => i.Title.Contains(filter.title));
                }
                if (filter.findRootItem != null && filter.findRootItem == true)
                {
                    query = query.Where(i => i.ParentId == null || i.ParentId == Guid.Empty);
                }
                return Json(new AjaxResponse<List<CmsCategory>>
                {                    
                    data = query.ToList()
                });
            }
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
