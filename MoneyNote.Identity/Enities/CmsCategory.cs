using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyNote.Identity.Enities
{
    [Table("cmscategory")]
    public class CmsCategory: AbastractEntity
    {
        public string Title { get; set; }

        public class Dto
        {
            public Guid Id { get; set; }
            public Guid ParentId { get; set; }
            public string Title { get; set; }
        }
    }
}
