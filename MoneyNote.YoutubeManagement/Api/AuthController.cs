using Microsoft.AspNetCore.Mvc;
using MoneyNote.Identity.Enities;
using MoneyNote.Identity.PermissionSchemes;
using MoneyNote.YoutubeManagement.Api.Models;
using MoneyNote.YoutubeManagement.Models;

namespace MoneyNote.YoutubeManagement.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        [Route("Login")]
        public JsonResponse<string> Login(LoginRequest user)
        {
            var ok = Auth.Login(user.Username, user.Password, HttpContext, out string token);

            return new JsonResponse<string>
            {
                code = ok == true ? 0 : 1,
                message = ok == true ? "Logged" : "Wrong username or password",
                data = token
            };
        }

        [HttpPost]
        [Route("Logout")]
        public JsonResponse<bool> Logout()
        {
            Auth.Logout(HttpContext);

            return new JsonResponse<bool> { data = true };
        }
    }
}
