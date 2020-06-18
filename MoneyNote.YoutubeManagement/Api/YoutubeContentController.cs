using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyNote.Identity;
using MoneyNote.Identity.Enities;
using MoneyNote.YoutubeManagement.Models;
using MoneyNote.YoutubeManagement.Repository;

namespace MoneyNote.YoutubeManagement.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class YoutubeContentController : ControllerBase
    {
        public AjaxResponse<List<CmsCategory>> ListCategory(JsGridFilter filter)
        {
            filter = filter ?? new JsGridFilter();
            filter.categoryIds = filter.categoryIds ?? new List<Guid>();
            filter.categoryIds.Where(i => i != null && i != Guid.Empty).ToList();

            return new AjaxResponse<List<CmsCategory>>
            {
                data = new YoutubeContentRepository().ListCategory(filter).data
            };
        }

        public AjaxResponse<ContentJsGridResult> ListContent(JsGridFilter filter)
        {

            return new AjaxResponse<ContentJsGridResult>
            {
                data = new YoutubeContentRepository().ListContent(filter)
            };
        }

        public AjaxResponse<CmsContent> GetContent(ContentRequest request)
        {
            using (var db = new MoneyNoteDbContext())
            {
                var exited = db.CmsContents.FirstOrDefault(i => i.Id == request.id);
                if (exited != null)
                {
                    exited.CountView = exited.CountView + 1;
                    db.SaveChanges();
                }

                return new AjaxResponse<CmsContent> { data = exited };
            }
        }
    }
}
