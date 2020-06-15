using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MoneyNote.Identity;
using MoneyNote.Identity.Enities;
using MoneyNote.Identity.PermissionSchemes;
using MoneyNote.YoutubeManagement.Models;

namespace MoneyNote.YoutubeManagement.Controllers
{
    [ClaimAndValidatePermission("*", "CmsContent")]
    public class ContentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SelectAll([FromBody] JsGridFilter filter)
        {
            List<CmsContent> data = new List<CmsContent>();
            List<CmsCategory> listCategory = new List<CmsCategory>();
            List<CmsRelation> listRelation = new List<CmsRelation>();
            long itemsCount = 0;
            using (var db = new MoneyNoteDbContext())
            {            
                var query = db.CmsContents.AsQueryable();
                if (!string.IsNullOrEmpty(filter.title))
                {
                    query = query.Where(i => i.Title.Contains(filter.title));
                }
                if (!string.IsNullOrEmpty(filter.description))
                {
                    query = query.Where(i => i.Description.Contains(filter.description));
                }
                if (!string.IsNullOrEmpty(filter.urlRef))
                {
                    query = query.Where(i => i.UrlRef.Contains(filter.urlRef));
                }
                if (filter.categoryIds != null && filter.categoryIds.Count > 0)
                {
                    query = query.Join(db.CmsRelations, c => c.Id, r => r.ContentId, (c, r) => new { c, r })
                        .Where(m => filter.categoryIds.Contains(m.r.CategoryId))
                        .Select(m => m.c);
                }
                query = query.Distinct().OrderByDescending(i => i.CreatedAt);

                itemsCount = query.LongCount();
                data = query.Skip((filter.pageIndex - 1) * filter.pageSize).Take(filter.pageSize).ToList();

                listCategory = db.CmsCategories.Where(i => i.IsDeleted == 0).ToList();

                var contentIds = data.Select(i => i.Id).ToList();
                if (contentIds.Count > 0)
                {
                    listRelation = db.CmsRelations.Where(i => contentIds.Contains(i.ContentId)).ToList();
                }

                //cbeadc96-a21a-4ab8-a69b-8a56c893ffce
            }

            return Json(new AjaxResponse<JsGridResult<CmsContent>>
            {
                data = new JsGridResult<CmsContent>
                {
                    data = data,
                    itemsCount = itemsCount,
                    listRelation = listRelation,
                    listCategory = listCategory
                }
            });
        }


    }
}
