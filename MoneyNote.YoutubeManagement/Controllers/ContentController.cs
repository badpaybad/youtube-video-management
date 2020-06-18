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
using MoneyNote.YoutubeManagement.Repository;

namespace MoneyNote.YoutubeManagement.Controllers
{
    [ClaimAndValidatePermission("*", "CmsContent")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ContentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SelectAll([FromBody] JsGridFilter filter)
        {
          
            return Json(new AjaxResponse<ContentJsGridResult>
            {
                data = new YoutubeContentRepository().ListContent(filter)
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

                    CmsContent cmsContent = new CmsContent
                    {
                        Title = FindYoutubeContent(result, "<meta property=\"og:title\" content=\"", "\">"),
                        Thumbnail = FindYoutubeContent(result, "<meta property=\"og:image\" content=\"", "\">"),
                        UrlRef = FindYoutubeContent(result, "<meta property=\"og:url\" content=\"", "\">"),
                        Description = FindYoutubeContent(result, "\\\"description\\\":{\\\"simpleText\\\":\\\"", "\\\"}"),
                    };

                    if (request.autoSave)
                    {
                        using (var db = new MoneyNoteDbContext())
                        {
                            db.CmsContents.Add(cmsContent);
                            db.SaveChanges();
                        }
                    }

                    return base.Json(new AjaxResponse<CmsContent>
                    {
                        data = cmsContent
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
