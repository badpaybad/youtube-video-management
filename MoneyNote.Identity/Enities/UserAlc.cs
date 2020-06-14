using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyNote.Identity.Enities
{
    [Table("UserAlc")]
    public class UserAlc: AbastractEntity
    {
        [Column(TypeName ="varchar(36)")]
        public Guid UserId { get; set; }
        public string ModuleCode { get; set; }
        public string PermissionCode { get; set; }
    }
}
