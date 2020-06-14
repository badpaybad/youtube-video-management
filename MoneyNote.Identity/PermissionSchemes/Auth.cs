using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using MoneyNote.Core;
using MoneyNote.Identity.Enities;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.Unicode;

namespace MoneyNote.Identity.PermissionSchemes
{
    public class Auth
    {
        static Auth()
        {

        }

        public static void InitSupperAdmin()
        {
            using (var db = new MoneyNoteDbContext())
            {
                var user = db.Users.FirstOrDefault(i => i.Username.Equals("supperadmin"));
                if (user == null)
                {
                    db.Users.Add(new User
                    {
                        Username = "supperadmin",
                        Password = HashPassword("123@123")
                    });
                    db.SaveChanges();
                }
            }
        }

        public static string GenerateToken(string salt = "")
        {
            using (var sha = SHA512.Create())
            {
                return System.Convert.ToBase64String(sha.ComputeHash(UTF8Encoding.UTF8.GetBytes(salt + Guid.NewGuid().ToString())));
            }
        }

        public static string HashPassword(string src)
        {
            using (var sha = SHA512.Create())
            {
                return Convert.ToBase64String(sha.ComputeHash(UTF8Encoding.UTF8.GetBytes(src)));
            }
        }

        public static bool Login(string username, string password, HttpContext context, out string token)
        {
            User user;
            var encryptPwd = HashPassword(password);
            using (var db = new MoneyNoteDbContext())
            {
                user = db.Users.FirstOrDefault(i => i.Username.Equals(username) && i.Password.Equals(encryptPwd));
            }

            token = string.Empty;

            if (user == null) return false;

            token = GenerateToken(username);

            if (context != null)
            {
                context.Session.Set("__CurrentUserToken", UTF8Encoding.UTF8.GetBytes(token));
            }

            MemoryMessageBus.Instance.CacheSet(token, user, DateTime.Now.AddDays(1));
            if (user.Username == "supperadmin")
            {
                MemoryMessageBus.Instance.CacheSet($"{token}_module", new List<string> { "SupperAdmin" });
                MemoryMessageBus.Instance.CacheSet($"{token}_permission", new List<string> { "SupperAdmin" });
            }

            return true;
        }
        public static bool Logout(HttpContext context)
        {
            var token = GetToken(context);

            MemoryMessageBus.Instance.CacheRemove(token);
            MemoryMessageBus.Instance.CacheRemove($"{token}_module");
            MemoryMessageBus.Instance.CacheRemove($"{token}_permission");

            return true;
        }
        public static bool Logout(string token)
        {
            MemoryMessageBus.Instance.CacheRemove(token);
            MemoryMessageBus.Instance.CacheRemove($"{token}_module");
            MemoryMessageBus.Instance.CacheRemove($"{token}_permission");

            return true;
        }

        public static User Get(string token)
        {
            return MemoryMessageBus.Instance.CacheGet<User>(token);
        }

        public static User Get(HttpContext context)
        {
            return MemoryMessageBus.Instance.CacheGet<User>(GetToken(context));
        }

        public static string GetToken(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(token))
            {
                context.Session.TryGetValue("__CurrentUserToken", out byte[] utoken);
                if (utoken != null)
                {
                    token = UTF8Encoding.UTF8.GetString(utoken);
                }
            }

            //you may want check cookies if you do you own code to save token to cookies

            if (!string.IsNullOrEmpty(token) && token.StartsWith(JwtBearerDefaults.AuthenticationScheme, StringComparison.OrdinalIgnoreCase))
            {
                token = token.Substring(4).Trim();
            }

            return token;
        }

        public static bool IsSupperAdmin(string token)
        {
            var modules = MemoryMessageBus.Instance.CacheGet<List<string>>($"{token}_module");
            var permissions = MemoryMessageBus.Instance.CacheGet<List<string>>($"{token}_permission");

            return (modules.Any(i => i == "SupperAdmin") && permissions.Any(i => i == "SupperAdmin"));
        }

        public static HttpStatusCode ValidateModule(string token, params string[] codes)
        {
            var user = Auth.Get(token);

            if (user == null)
            {
                return HttpStatusCode.Unauthorized;
            }

            var modules = MemoryMessageBus.Instance.CacheGet<List<string>>($"{token}_module");
            var permissions = MemoryMessageBus.Instance.CacheGet<List<string>>($"{token}_permission");

            var isSupperAdmin = (modules.Any(i => i == "SupperAdmin") && permissions.Any(i => i == "SupperAdmin"));

            if (isSupperAdmin) return HttpStatusCode.OK;

            //TODO: check module code with user loged compare to codes 

            return HttpStatusCode.OK;
        }

        public static HttpStatusCode ValidatePermission(string token, params string[] codes)
        {
            var user = Auth.Get(token);

            if (user == null)
            {
                return HttpStatusCode.Unauthorized;
            }

            var modules = MemoryMessageBus.Instance.CacheGet<List<string>>($"{token}_module");
            var permissions = MemoryMessageBus.Instance.CacheGet<List<string>>($"{token}_permission");

            var isSupperAdmin = (modules.Any(i => i == "SupperAdmin") && permissions.Any(i => i == "SupperAdmin"));
            if (isSupperAdmin) return HttpStatusCode.OK;

            //TODO: check module code with user loged compare to codes 

            return HttpStatusCode.OK;
        }
    }
}
