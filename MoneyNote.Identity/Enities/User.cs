using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using System.Text;

namespace MoneyNote.Identity.Enities
{
    [Table("user")]
    public class User : AbastractEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime? LastLogedin { get; set; }

        public string LastToken { get; set; }

        [NotMapped]
        public List<UserAcl.Dto>? Acls { get; set; } = new List<UserAcl.Dto>();

        public class Dto
        {
            public Guid Id { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public DateTime? LastLogedin { get; set; }

            public string LastToken { get; set; }

            public List<UserAcl.Dto>? Acls { get; set; } = new List<UserAcl.Dto>();
        }
    }

}
