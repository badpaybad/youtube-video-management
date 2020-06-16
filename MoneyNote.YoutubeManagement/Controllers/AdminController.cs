using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyNote.Identity;
using MoneyNote.Identity.Enities;
using MoneyNote.Identity.PermissionSchemes;
using MoneyNote.YoutubeManagement.Models;

namespace MoneyNote.YoutubeManagement.Controllers
{
    [ClaimAndValidatePermission("*", "AdminDashboard")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SelectAllPermission()
        {
            using (var db = new MoneyNoteDbContext())
            {
                var query = db.SysPermissions.OrderBy(i=>i.ModuleCode).ThenBy(i=>i.Code).Where(i => i.IsDeleted == 0);

                return Json(new AjaxResponse<List<SysPermission>>
                {
                    data = query.ToList()
                });
            }
        }

        public IActionResult SelectAll([FromBody] JsGridFilter filter)
        {
            filter = filter ?? new JsGridFilter();
            filter.categoryIds = filter.categoryIds ?? new List<Guid>();
            filter.categoryIds.Where(i => i != null && i != Guid.Empty).ToList();
            long itemsCount = 0;
            List<User> data = new List<User>();

            List<UserAcl.Dto> acls = new List<UserAcl.Dto>();
            using (var db = new MoneyNoteDbContext())
            {
                var query = db.Users.Where(i => i.IsDeleted == 0);
                if (!string.IsNullOrEmpty(filter.title))
                {
                    query = query.Where(i => i.Username.Contains(filter.title));
                }
                if (filter.findRootItem != null && filter.findRootItem == true)
                {
                    query = query.Where(i => i.ParentId == null || i.ParentId == Guid.Empty);
                }

                if (!string.IsNullOrEmpty(filter.moduleCode))
                {
                    query = query.Join(db.UserAcls, u => u.Id, acl => acl.UserId, (u, acl) => new { u, acl })
                        .Where(i => i.acl.ModuleCode == filter.moduleCode).Select(i => i.u).Distinct();
                }
                if (!string.IsNullOrEmpty(filter.permissionCode))
                {
                    query = query.Join(db.UserAcls, u => u.Id, acl => acl.UserId, (u, acl) => new { u, acl })
                        .Where(i => i.acl.PermissionCode == filter.permissionCode).Select(i => i.u).Distinct();
                }
                query = query.Distinct().OrderByDescending(i => i.CreatedAt);

                itemsCount = query.LongCount();
                data = query.Skip((filter.pageIndex - 1) * filter.pageSize).Take(filter.pageSize).ToList();

                var userIds = data.Select(i => i.Id).Distinct().ToList();

                acls = db.UserAcls.Where(i => userIds.Contains(i.UserId))
                    .Select(i => new UserAcl.Dto
                    {
                        UserId = i.UserId,
                        ModuleCode = i.ModuleCode,
                        PermissionCode = i.PermissionCode
                    })
                    .Distinct().ToList();

                return Json(new AjaxResponse<UserJsGridResult>
                {
                    data = new UserJsGridResult
                    {
                        data = data,
                        itemsCount = itemsCount,
                        ListUserAcl= acls
                    }
                });
            }
        }

        public IActionResult CreateOrUpdate([FromBody] User data)
        {
            if (string.IsNullOrEmpty(data.Username)) return Json(new AjaxResponse<string> { code = 1, message = "Usenam can not be empty" });
            if (data.Id == null || data.Id == Guid.Empty) data.Id = Guid.NewGuid();

            var passwordHashed = string.Empty;

            if (!string.IsNullOrEmpty(data.Password))
            {
                passwordHashed = Auth.HashPassword(data.Password);
            }

            using (var db = new MoneyNoteDbContext())
            {
                var exited = db.Users.FirstOrDefault(i => i.Username == data.Username && i.IsDeleted == 0);
                if (exited == null)
                {
                    db.Users.Add(data);
                }
                else
                {
                    if (!string.IsNullOrEmpty(passwordHashed))
                    {
                        exited.Password = passwordHashed;
                    }

                }

                db.SaveChanges();
            }

            using (var db = new MoneyNoteDbContext())
            {
                var existed = db.UserAcls.Where(i => i.UserId == data.Id).ToList();
                db.UserAcls.RemoveRange(existed);
                db.SaveChanges();
            }
            if (data.Acls != null && data.Acls.Count > 0)
            {
                List<UserAcl> acls = data.Acls.Distinct().Select(i => new UserAcl
                {
                    Id = Guid.NewGuid(),
                    ModuleCode = i.ModuleCode,
                    PermissionCode = i.PermissionCode,
                    UserId = data.Id
                }).ToList();
                using (var db = new MoneyNoteDbContext())
                {
                    db.UserAcls.AddRange(acls);
                    db.SaveChanges();
                }
            }

            data.Password = string.Empty;

            return Json(new AjaxResponse<User> { data = data });
        }

        public IActionResult Delete([FromBody] User data)
        {
            using (var db = new MoneyNoteDbContext())
            {
                data = db.Users.FirstOrDefault(i => i.Id == data.Id && i.IsDeleted == 0);
                if (data != null)
                {
                    data.IsDeleted = 1;
                    db.SaveChanges();
                }
            }
            return Json(new AjaxResponse<User> { data = data });
        }
    }
}
