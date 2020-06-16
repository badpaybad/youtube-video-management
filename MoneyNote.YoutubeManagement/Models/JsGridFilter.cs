using MoneyNote.Identity.Enities;
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
        public string title { get; set; }
        public string thumbnail { get; set; }
        public string description { get; set; }
        public string urlRef { get; set; }

        public List<Guid>? categoryIds { get; set; } = new List<Guid>();
        public bool? findRootItem { get; set; }
        
    }

    public class JsGridResult<T>
    {
        public List<T> data { get; set; }
        public long itemsCount { get; set; }

        //public List<CmsCategory> listCategory { get; set; }
        public List<CmsRelation> listRelation { get; set; }
    }
}
