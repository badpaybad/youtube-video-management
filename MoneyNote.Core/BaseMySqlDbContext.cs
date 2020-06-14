using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MoneyNote.Core
{
    public class BaseMySqlDbContext : DbContext
    {
        public BaseMySqlDbConnection _dbConnect;

        public BaseMySqlDbContext(string connectionStringOrConnectionName)
        {
            _dbConnect = new BaseMySqlDbConnection(connectionStringOrConnectionName);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(_dbConnect.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public void SaveChangeWith_BulkInsert<TEntity>(List<TEntity> allItems, int batchSize = 10) where TEntity : class
        {
            var skip = 0;
            while (true)
            {
                var batch = allItems.Skip(skip).Take(batchSize).Distinct().ToList();

                if (batch == null || batch.Count == 0) { return; }

                this.Set<TEntity>().AddRange(batch);
                this.SaveChanges();

                skip = skip + batchSize;

            }
        }

        public void SaveChangeWith_ModifiedFields<TEntity>(List<TEntity> list
         , params Expression<Func<TEntity, object>>[] modifiedFields) where TEntity : class
        {
            if (list == null || list.Count == 0) return;

            if (modifiedFields == null || modifiedFields.Count() == 0)
            {
                foreach (var l in list)
                {
                    var entry = this.Entry(l);

                    entry.State = EntityState.Modified;
                }
            }
            else
            {
                var listNameModified = new List<string>();
                foreach (var f in modifiedFields)
                {
                    var expression = f.Body as MemberExpression;
                    if (expression == null)
                    {
                        var u = f.Body as UnaryExpression;
                        expression = u.Operand as MemberExpression;
                    }
                    listNameModified.Add(expression.Member.Name);
                }

                foreach (var l in list)
                {
                    var entry = Entry(l);

                    entry.State = EntityState.Modified;

                    foreach (var p in entry.Properties)
                    {
                        if (listNameModified.Contains(p.Metadata.Name))
                        {
                            p.IsModified = true;
                        }
                        else
                        {
                            p.IsModified = false;
                        }
                    }
                }
            }

            this.SaveChanges();
        }

        public void SaveChangeWith_UnmodifiedFields<TEntity>(List<TEntity> list
          , params Expression<Func<TEntity, object>>[] unmodifiedFields) where TEntity : class
        {
            if (list == null || list.Count == 0) return;

            if (unmodifiedFields == null || unmodifiedFields.Count() == 0)
            {
                foreach (var l in list)
                {
                    var entry = this.Entry(l);

                    entry.State = EntityState.Modified;
                }
            }
            else
            {
                var listNameUnmodified = new List<string>();
                foreach (var f in unmodifiedFields)
                {
                    var expression = f.Body as MemberExpression;
                    if (expression == null)
                    {
                        var u = f.Body as UnaryExpression;
                        expression = u.Operand as MemberExpression;
                    }
                    listNameUnmodified.Add(expression.Member.Name);
                }

                foreach (var l in list)
                {
                    var entry = this.Entry(l);

                    entry.State = EntityState.Modified;

                    foreach (var p in entry.Properties)
                    {
                        if (listNameUnmodified.Contains(p.Metadata.Name))
                        {
                            p.IsModified = false;
                        }
                        else
                        {
                            p.IsModified = true;
                        }
                    }
                }
            }

            this.SaveChanges();
        }

        /// <summary>
        /// map column(s) name the same property(s) name of TEnity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public List<TEntity> Select<TEntity>(string cmdText) where TEntity : class
        {
            return _dbConnect.Select<TEntity>(cmdText);
        }

        public int ExecuteNonQuery(string cmdText)
        {
            return _dbConnect.ExecuteNonQuery(cmdText);
        }
    }

}
