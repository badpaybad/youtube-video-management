﻿using System;
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

            if (string.IsNullOrEmpty(request.SortType) == false && request.SortType.IndexOf("random") >= 0)
            {
                request.SortType = _rnd.Next(1, 1000000) % 2 == 0 ? "oldest" : "newest";
            }

            using (var db = new MoneyNoteDbContext())
            {
                var query = db.CmsContents.Where(i=>i.IsPublished==1).Join(db.CmsRelations, c => c.Id, r => r.ContentId, (c, r) => new { c, r });
                if (string.IsNullOrEmpty(request.Type) == false && request.Type.IndexOf("image") >= 0)
                {
                    query = query.Where(i => i.c.UrlRef == string.Empty || i.c.UrlRef == null);
                }
                if(string.IsNullOrEmpty(request.Type) == false && request.Type.IndexOf("video") >= 0)
                {
                    query = query.Where(i => i.c.UrlRef != string.Empty && i.c.UrlRef != null);
                }

                var queryCat = query.AsQueryable();

                if (request.ContentId != null && request.ContentId != Guid.Empty)
                {
                    queryCat = queryCat.Where(i => i.r.ContentId == request.ContentId);
                }
                var categories = queryCat.Select(i => i.r.CategoryId).Distinct().ToList();

                var relation = queryCat.Select(i => new CmsRelation.Dto { CategoryId = i.r.CategoryId, ContentId = i.r.ContentId })
                        .Distinct().ToList();

                var queryContent = query.Where(i => i.c.Id != request.ContentId)
                    .Where(i => categories.Contains(i.r.CategoryId)).Select(i => i.c).Distinct();

                if (string.IsNullOrEmpty(request.Type) == false && request.SortType.IndexOf("oldest") >= 0)
                {
                    queryContent = queryContent.OrderBy(i => i.CreatedAt);
                }
                else
                {
                    queryContent = queryContent.OrderByDescending(i => i.CreatedAt);
                }
             
                var total = queryContent.Count();

                if (request.pageIndex != null && request.pageSize > 0)
                {
                    int skip = (request.pageIndex.Value - 1) * request.pageSize.Value;
                    skip = skip < 0 ? 0 : skip;

                    int take = request.pageSize.Value;
                    queryContent = queryContent.Skip(skip).Take(take);
                }

                var data = queryContent.ToList();

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
