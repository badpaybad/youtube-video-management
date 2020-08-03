using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyNote.Identity;
using MoneyNote.Identity.Enities;
using MoneyNote.Identity.PermissionSchemes;
using MoneyNote.YoutubeManagement.Api.Models;
using MoneyNote.YoutubeManagement.Models;
using MoneyNote.YoutubeManagement.Repository;
using Org.BouncyCastle.Ocsp;

namespace MoneyNote.YoutubeManagement.Api
{
    [Route("api/[controller]")]
    [ApiController]
    //[ClaimAndValidatePermission("*", "ApiYoutubeContent", true)]
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class YoutubeContentController : ControllerBase
    {
        static Random _rnd = new Random();
        [Route("ListCategory")]
        [HttpPost]
        public JsonResponse<CategoryJsGridResult> ListCategory(JsGridFilter filter)
        {
            filter = filter ?? new JsGridFilter();
            filter.categoryIds = filter.categoryIds ?? new List<Guid>();
            filter.categoryIds.Where(i => i != null && i != Guid.Empty).ToList();

            return new JsonResponse<CategoryJsGridResult>
            {
                data = new YoutubeContentRepository().ListCategory(filter)
            };
        }

        [Route("ListContent")]
        [HttpPost]
        public JsonResponse<ContentJsGridResult> ListContent(JsGridFilter filter)
        {

            return new JsonResponse<ContentJsGridResult>
            {
                data = new YoutubeContentRepository().ListContent(filter, true)
            };
        }
        [Route("GetContent")]
        [HttpPost]
        public JsonResponse<CmsContent> GetContent(ContentRequest request)
        {
            using (var db = new MoneyNoteDbContext())
            {
                var exited = db.CmsContents.FirstOrDefault(i => i.Id == request.id);
                if (exited != null)
                {
                    exited.CountView = exited.CountView + 1;
                    db.SaveChanges();
                }

                return new JsonResponse<CmsContent> { data = exited };
            }
        }


        [Route("ListContentRelated")]
        [HttpPost]
        public JsonResponse<ContentJsGridResult> GetContentRelated(ContentRelatedRequest request)
        {
            request.Type = (request.Type ?? string.Empty).ToLower();
            request.SortType = (request.SortType ?? string.Empty).ToLower();

            if (request.SortType.IndexOf("random") >= 0)
            {
                request.SortType = _rnd.Next(1, 1000000) % 2 == 0 ? "oldest" : "newest";
            }

            using (var db = new MoneyNoteDbContext())
            {
                var query = db.CmsContents.Join(db.CmsRelations, c => c.Id, r => r.ContentId, (c, r) => new { c, r });
                if (request.Type.IndexOf("image") >= 0)
                {
                    query = query.Where(i => i.c.UrlRef == string.Empty || i.c.UrlRef == null);
                }
                if (request.ContentId != null && request.ContentId != Guid.Empty)
                {
                    query = query.Where(i => i.r.ContentId == request.ContentId);
                }

                var categories = query.Select(i => i.r.CategoryId).Distinct().ToList();

                var relation = query.Select(i => new CmsRelation.Dto { CategoryId = i.r.CategoryId, ContentId = i.r.ContentId })
                        .Distinct().ToList();

                var tempQuery = query.Where(i => i.c.Id != request.ContentId)
                    .Where(i => categories.Contains(i.r.CategoryId));

                if (request.SortType.IndexOf("oldest") >= 0)
                {
                    tempQuery = tempQuery.OrderBy(i => i.c.CreatedAt);
                }
                else
                {
                    tempQuery = tempQuery.OrderByDescending(i => i.c.CreatedAt);
                }

                var total = tempQuery.Count();

                if (request.pageIndex != null && request.pageSize > 0)
                {
                    int skip = (request.pageIndex.Value - 1) * request.pageSize.Value;
                    skip = skip < 0 ? 0 : skip;

                    int take = request.pageSize.Value;
                    tempQuery = tempQuery.Skip(skip).Take(take);
                }

                var data = tempQuery.Select(i => i.c).ToList();

                return new JsonResponse<ContentJsGridResult>
                {
                    data = new ContentJsGridResult
                    {
                        data = data,
                        itemsCount = total,
                        listRelation= relation
                    },
                };
            }
        }

    }
}
