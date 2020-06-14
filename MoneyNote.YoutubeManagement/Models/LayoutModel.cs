using Microsoft.AspNetCore.Http;
using MoneyNote.Identity.Enities;
using MoneyNote.Identity.PermissionSchemes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MoneyNote.YoutubeManagement.Models
{
    public class LayoutModel
    {
        public User User { get; set; }

        public static LayoutModel Init(HttpContext context)
        {
            return new LayoutModel
            {
                User = Auth.Get(context)
            };
        }
    }
}
