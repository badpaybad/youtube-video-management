using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoneyNote.Identity.Enities
{
    [Table("SysModule")]
    public class SysModule : AbastractEntity
    {
        public string Code { get; set; }
    }
}
