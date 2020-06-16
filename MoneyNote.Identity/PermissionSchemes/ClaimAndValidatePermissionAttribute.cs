using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace MoneyNote.Identity.PermissionSchemes
{
    public class ClaimAndValidatePermissionAttribute : Attribute, IAuthorizationFilter
    {
        string _permissionCode;
        string _moduleCode;
        bool _isApi;
        public ClaimAndValidatePermissionAttribute(string permissionCode, string moduleCode, bool isApi = false)
        {
            _permissionCode = permissionCode;
            _moduleCode = moduleCode;
            _isApi = isApi;

            Auth.CreateOrUpdateSysModule(moduleCode);
            Auth.CreateOrUpdateSysPermission(permissionCode, moduleCode);
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                return;
            }

            var token = Auth.GetToken(context.HttpContext);

            System.Net.HttpStatusCode httpStatusCode = Auth.ValidatePermission(token, _permissionCode, _moduleCode);
            context.HttpContext.Response.StatusCode = (int)httpStatusCode;

            if (httpStatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                if (!_isApi
                    && ( context.HttpContext.Request.Headers.ContainsKey("RequestSourceType") == false
                    || ((string)context.HttpContext.Request.Headers["RequestSourceType"]) == ""
                    || ((string)context.HttpContext.Request.Headers["RequestSourceType"]) == "web"))
                {
                    context.Result = new RedirectResult(Auth.LoginUrl);
                }
                else
                {
                    context.Result = new UnauthorizedResult();
                }
            }
        }
    }
}
