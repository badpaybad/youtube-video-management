using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoneyNote.Identity.Enities
{
    public abstract class AbastractEntity
    {
        [Key]
        [Column(TypeName ="varchar(36)")]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Column(TypeName = "varchar(36)")]
        public Guid ParentId { get; set; } = Guid.Empty;
        [Column(TypeName = "varchar(36)")]
        public Guid TenantId { get; set; } = Guid.Empty;
        public int IsDeleted { get; set; }
        [Column(TypeName = "varchar(36)")]
        public Guid CreatedBy { get; set; } = Guid.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Column(TypeName = "varchar(36)")]
        public Guid UpdatedBy { get; set; } = Guid.Empty;
        public DateTime? UpdatedAt { get; set; }
    }
}
