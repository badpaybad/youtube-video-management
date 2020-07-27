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
using MoneyNote.YoutubeManagement.Models;
using MoneyNote.YoutubeManagement.Repository;

namespace MoneyNote.YoutubeManagement.Api
{
    [Route("api/[controller]")]
    [ApiController]
    //[ClaimAndValidatePermission("*", "ApiYoutubeContent", true)]
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = false)]   
    public class YoutubeContentController : ControllerBase
    {
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
    }
}
