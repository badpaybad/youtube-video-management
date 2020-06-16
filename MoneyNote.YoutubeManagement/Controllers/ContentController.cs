using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
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
            filter = filter ?? new JsGridFilter();
            filter.categoryIds = filter.categoryIds ?? new List<Guid>();
            filter.categoryIds.Where(i => i != null && i != Guid.Empty).ToList();
            long itemsCount = 0;
            List<CmsContent> data = new List<CmsContent>();
            //List<CmsCategory> listCategory = new List<CmsCategory>();
            List<CmsRelation.Dto> listRelation = new List<CmsRelation.Dto>();

            using (var db = new MoneyNoteDbContext())
            {
                var query = db.CmsContents.Where(i => i.IsDeleted == 0);
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
                if (filter.findRootItem != null && filter.findRootItem == true)
                {
                    query = query.Where(i => i.ParentId == null || i.ParentId == Guid.Empty);
                }
                query = query.Distinct().OrderByDescending(i => i.CreatedAt);

                itemsCount = query.LongCount();
                data = query.Skip((filter.pageIndex - 1) * filter.pageSize).Take(filter.pageSize).ToList();

                //listCategory = db.CmsCategories.Where(i => i.IsDeleted == 0).ToList();

                var contentIds = data.Select(i => i.Id).ToList();
                if (contentIds.Count > 0)
                {
                    listRelation = db.CmsRelations.Where(i => contentIds.Contains(i.ContentId))
                        .Select(i => new CmsRelation.Dto { CategoryId = i.CategoryId, ContentId = i.ContentId })
                        .ToList();
                }

                //cbeadc96-a21a-4ab8-a69b-8a56c893ffce
            }

            return Json(new AjaxResponse<ContentJsGridResult>
            {
                data = new ContentJsGridResult
                {
                    data = data,
                    itemsCount = itemsCount,
                    listRelation = listRelation,
                    //listCategory = listCategory
                }
            });
        }

        public IActionResult CreateOrUpdate([FromBody] CmsContent data)
        {
            if (string.IsNullOrEmpty(data.Title)) return Json(new AjaxResponse<string> { code = 1, message = "Title can not be empty" });
            if (data.Id == null || data.Id == Guid.Empty) data.Id = Guid.NewGuid();

            using (var db = new MoneyNoteDbContext())
            {
                var exited = db.CmsContents.FirstOrDefault(i => i.Id == data.Id && i.IsDeleted == 0);
                if (exited == null)
                {
                    db.CmsContents.Add(data);
                }
                else
                {
                    exited.ParentId = data.ParentId;
                    exited.Title = data.Title;
                    exited.Thumbnail = data.Thumbnail;
                    exited.UrlRef = data.UrlRef;
                    exited.Description = data.Description;
                }

                db.SaveChanges();

                var existed = db.CmsRelations.Where(i => i.ContentId == data.Id).ToList();
                db.RemoveRange(existed);
                db.SaveChanges();

                db.AddRange(data.CategoryIds.Select(c => new CmsRelation
                {
                    ContentId = data.Id,
                    CategoryId = c
                }).ToList());
                db.SaveChanges();
            }

            return Json(new AjaxResponse<CmsContent> { data = data });
        }


        public IActionResult Delete([FromBody] CmsContent data)
        {
            using (var db = new MoneyNoteDbContext())
            {
                data = db.CmsContents.FirstOrDefault(i => i.Id == data.Id && i.IsDeleted == 0);
                if (data != null)
                {
                    data.IsDeleted = 1;
                    db.SaveChanges();
                }
            }
            return Json(new AjaxResponse<CmsContent> { data = data });
        }

        public IActionResult UpdateRelation([FromBody] ContentRelationModel data)
        {
            using (var db = new MoneyNoteDbContext())
            {
                var existed = db.CmsRelations.Where(i => i.ContentId == data.ContentId).ToList();
                db.RemoveRange(existed);
                db.SaveChanges();

                db.AddRange(data.CategoryIds.Select(c => new CmsRelation
                {
                    ContentId = data.ContentId,
                    CategoryId = c
                }).ToList());
                db.SaveChanges();
            }

            return Json(new AjaxResponse<ContentRelationModel> { data = data });
        }

        public IActionResult YoutubeCrawl([FromBody] YoutubeCrawlRequest request)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {

                    httpClient.BaseAddress = new Uri("https://www.youtube.com/");

                    var result = httpClient.GetStringAsync(request.url).GetAwaiter().GetResult();

                    return Json(new AjaxResponse<CmsContent>
                    {
                        data = new CmsContent
                        {
                            Title = FindYoutubeContent(result, "<meta property=\"og:title\" content=\"", "\">"),
                            Thumbnail = FindYoutubeContent(result, "<meta property=\"og:image\" content=\"", "\">"),
                            UrlRef = FindYoutubeContent(result, "<meta property=\"og:url\" content=\"", "\">"),
                            Description = FindYoutubeContent(result, "\\\"description\\\":{\\\"simpleText\\\":\\\"", "\\\"}"),
                        }
                    });
                }

            }
            catch (Exception ex)
            {
                return Json(new AjaxResponse<CmsContent>
                {
                    code = 1,
                    message = ex.Message
                });
            }

        }

        string FindYoutubeContent(string src, string begin, string end)
        {
            var idx = src.IndexOf(begin);
            if (idx < 0) return string.Empty;

            var temp = src.Substring(idx + begin.Length);
            idx = temp.IndexOf(end);

            return temp.Substring(0, idx).Replace("\\\\n"," ").Replace("\\n", " ").Replace("\\\\", " ").Replace("\\", " ");
        }
    }
}
