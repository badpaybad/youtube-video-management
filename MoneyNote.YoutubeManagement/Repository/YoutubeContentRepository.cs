using MoneyNote.Identity;
using MoneyNote.Identity.Enities;
using MoneyNote.YoutubeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyNote.YoutubeManagement.Repository
{
    public class YoutubeContentRepository
    {
        public CategoryJsGridResult ListCategory(JsGridFilter filter)
        {
            filter = filter ?? new JsGridFilter();
            filter.categoryIds = filter.categoryIds ?? new List<Guid>();
            filter.categoryIds.Where(i => i != null && i != Guid.Empty).ToList();
            List<CmsCategory> lst;
            using (var db = new MoneyNoteDbContext())
            {
                var query = db.CmsCategories.AsQueryable();
                if (!string.IsNullOrEmpty(filter.title))
                {
                    query = query.Where(i => i.Title.Contains(filter.title));
                }
                if (filter.findRootItem != null && filter.findRootItem == true)
                {
                    query = query.Where(i => i.ParentId == null || i.ParentId == Guid.Empty);
                }
                lst = query.ToList();

                return new CategoryJsGridResult { data = lst, itemsCount = lst.Count };
            }
        }

        public ContentJsGridResult ListContent(JsGridFilter filter, bool onlyPublished = false)
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
                var query = db.CmsContents.AsQueryable();
                if (onlyPublished)
                {
                    query = query.Where(i => i.IsDeleted == 0);
                }
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
                if (filter.contentId != null && filter.contentId.Value != Guid.Empty)
                {
                    var id = filter.contentId.Value;
                    query = query.Where(i => i.Id == id);
                }
                query = query.Distinct().OrderByDescending(i => i.CreatedAt);

                itemsCount = query.LongCount();

                if (filter.pageSize != 0)
                    query = query.Skip((filter.pageIndex - 1) * filter.pageSize).Take(filter.pageSize);

                data = query.ToList();

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

            return new ContentJsGridResult
            {
                data = data,
                itemsCount = itemsCount,
                listRelation = listRelation,
                //listCategory = listCategory
            };
        }

    }
}
