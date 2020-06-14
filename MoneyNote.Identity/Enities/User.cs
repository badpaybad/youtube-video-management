using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoneyNote.Identity.Enities
{
    [Table("User")]
    public  class User: AbastractEntity
    {       
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
