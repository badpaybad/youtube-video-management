using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace MoneyNote.Identity.PermissionSchemes
{
    public class PermissionDescriptorAttribute : Attribute, IAuthorizationFilter
    {
        string _code;
        bool _isApi;
        public PermissionDescriptorAttribute(string code, bool isApi = false)
        {
            _code = code;
            _isApi = isApi;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                return;
            }

            var token = Auth.GetToken(context.HttpContext);

            System.Net.HttpStatusCode httpStatusCode = Auth.ValidatePermission(_code);
            context.HttpContext.Response.StatusCode = (int)httpStatusCode;

            if (httpStatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                if (_isApi)
                {
                    context.Result = new UnauthorizedResult();
                }
                else
                {
                    context.Result = new RedirectResult("/Login");
                }
            }
        }
    }
}
