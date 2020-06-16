using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyNote.Identity.Enities
{
    [Table("CmsRelation")]
    public class CmsRelation : AbastractEntity
    {
        [Column(TypeName = "varchar(36)")]
        public Guid CategoryId { get; set; } = Guid.Empty;

        [Column(TypeName = "varchar(36)")]
        public Guid ContentId { get; set; } = Guid.Empty;

        public class Dto
        {
            public Guid CategoryId { get; set; }
            public Guid ContentId { get; set; }
        }
    }
}
