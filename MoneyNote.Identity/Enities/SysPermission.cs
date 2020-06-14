using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyNote.Identity.Enities
{
    [Table("SysPermission")]
    public class SysPermission : AbastractEntity
    {
        public string Code { get; set; }
        public string ModuleCode { get; set; }
    }
}
