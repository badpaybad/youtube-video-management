using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyNote.Identity.Enities
{
    [Table("cmscontent")]
    public class CmsContent : AbastractEntity
    {
        public string Title { get; set; }
        public string Thumbnail { get; set; }
        
        public int ThumbnailWidth { get; set; }
        public int ThumbnailHeight { get; set; }

        public int VideoWidth { get; set; }
        public int VideoHeight { get; set; }

        public string Description { get; set; }
        public string UrlRef { get; set; }

        public long? CountView { get; set; } = 0;

        public int IsPublished { get; set; }

        [NotMapped]
        public List<Guid>? CategoryIds { get; set; }

        public class Dto
        {
            public Guid Id { get; set; }
            public Guid ParentId { get; set; }
            public string Title { get; set; }
            public string Thumbnail { get; set; }
            public int ThumbnailWidth { get; set; }
            public int ThumbnailHeight { get; set; }

            public int VideoWidth { get; set; }
            public int VideoHeight { get; set; }

            public string Description { get; set; }
            public string UrlRef { get; set; }

            public long? CountView { get; set; } = 0;
            public bool IsPublished { get; set; }

            [NotMapped]
            public List<Guid>? CategoryIds { get; set; }
        }

    }
}
