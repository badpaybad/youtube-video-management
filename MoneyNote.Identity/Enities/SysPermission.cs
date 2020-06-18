using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyNote.Identity.Enities
{
    [Table("syspermission")]
    public class SysPermission : AbastractEntity
    {
        public string Code { get; set; }
        public string ModuleCode { get; set; }
        public class Dto
        {
            public string Code { get; set; }
            public string ModuleCode { get; set; }
        }
    }
}
