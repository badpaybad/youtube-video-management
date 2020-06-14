using Microsoft.EntityFrameworkCore;
using MoneyNote.Core;
using MoneyNote.Identity.Enities;
using System;

namespace MoneyNote.Identity
{
    public class MoneyNoteDbContext : BaseMySqlDbContext
    {
        public MoneyNoteDbContext() : base("MoneyNoteDbContext")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<CmsCategory> CmsCategories { get; set; }
        public DbSet<CmsContent> CmsContents { get; set; }
        public DbSet<CmsModule> CmsModules { get; set; }
        public DbSet<CmsPermission> CmsPermissions { get; set; }

    }
}
