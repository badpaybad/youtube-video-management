using Microsoft.AspNetCore.Http;
using MoneyNote.Core;
using MoneyNote.Identity.PermissionSchemes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoneyNote.Identity.Middleware
{
   public class IdentityMiddleware
    {
        private readonly RequestDelegate _next;

        public IdentityMiddleware( RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {            
           

            await _next.Invoke(context);

            // Clean up.
        }
    }
}
