using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MoneyNote.Identity.Enities
{
    [Table("cmscategory")]
    public class CmsCategory: AbastractEntity
    {
        public string Title { get; set; }
        public int ItemsCount { get; set; }
        public class Dto
        {
            public Guid Id { get; set; }
            public Guid? ParentId { get; set; }
            public string Title { get; set; }
            public int ItemsCount { get; set; }

            
        }
    }
}
