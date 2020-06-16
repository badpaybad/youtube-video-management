using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyNote.Identity.Enities
{
    [Table("useracl")]
    public class UserAcl: AbastractEntity
    {
        [Column(TypeName ="varchar(36)")]   
        public Guid UserId { get; set; }
        public string ModuleCode { get; set; }
        public string PermissionCode { get; set; }

        public class Dto
        {
            public Guid UserId { get; set; }
            public string ModuleCode { get; set; }
            public string PermissionCode { get; set; }
        }
    }
}
