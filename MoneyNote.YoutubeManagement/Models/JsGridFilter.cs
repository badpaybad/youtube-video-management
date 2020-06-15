using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyNote.YoutubeManagement.Models
{
    public class JsGridFilter
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public Guid parentId { get; set; }
        public string title { get; set; }
        public string thumbnail { get; set; }
        public string description { get; set; }
        public string urlRef { get; set; }
    }
}
