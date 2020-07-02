﻿using MoneyNote.Identity.Enities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyNote.YoutubeManagement.Models
{
    public class JsGridFilter
    {
        public int pageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public Guid? parentId { get; set; } = Guid.Empty;
        public Guid? contentId { get; set; } = Guid.Empty;
        public List<Guid>? categoryIds { get; set; } = new List<Guid>();
        public bool? findRootItem { get; set; } = false;
        public string title { get; set; }
        public string thumbnail { get; set; }
        public string description { get; set; }
        public string urlRef { get; set; }
        public string moduleCode { get; set; }
        public string permissionCode { get; set; }        
    }

    public interface IJsGridResult<T>
    {
        public List<T> data { get; set; }
        public long itemsCount { get; set; }
    }
    public class CategoryJsGridResult : IJsGridResult<CmsCategory>
    {
        public List<CmsCategory> data { get; set; }
        public long itemsCount { get; set; }

    }
    public class ContentJsGridResult: IJsGridResult<CmsContent>
    {
        public List<CmsContent> data { get; set; }
        public long itemsCount { get; set; }

        //public List<CmsCategory> listCategory { get; set; }
        public List<CmsRelation.Dto> listRelation { get; set; }
    }

    public class UserJsGridResult : IJsGridResult<User>
    {
        public List<User> data { get; set; }
        public long itemsCount { get; set; }

        //public List<CmsCategory> listCategory { get; set; }
        public List<UserAcl.Dto> ListUserAcl { get; set; }
    }

    public class ContentRequest
    {
        public Guid id { get; set; }
    }
}
