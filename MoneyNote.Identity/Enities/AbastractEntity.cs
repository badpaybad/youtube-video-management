using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoneyNote.Identity.Enities
{
    public abstract class AbastractEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public Guid TenantId { get; set; }
        public bool IsDeleted { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
