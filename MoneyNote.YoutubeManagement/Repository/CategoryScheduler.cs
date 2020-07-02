using MoneyNote.Identity;
using MoneyNote.Identity.Enities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyNote.YoutubeManagement.Repository
{
    public class CategoryScheduler : IDisposable
    {
        static Thread _thread;
        static bool _isStop = false;
        static CategoryScheduler()
        {
            _thread = new Thread(Loop);
            _thread.Start();
        }

        static void Loop()
        {
            while (!_isStop)
            {
                try
                {
                    List<CmsCategory> cats;
                    using (var db = new MoneyNoteDbContext())
                    {
                        cats = db.CmsCategories.ToList();

                        foreach (var c in cats)
                        {
                            c.ItemsCount = db.CmsRelations.Count(i => i.CategoryId == c.Id);
                        }
                    }
                    using (var db = new MoneyNoteDbContext())
                    {
                        db.SaveChangeWith_ModifiedFields(cats, i => i.ItemsCount);
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    Thread.Sleep(60 * 1000);
                }
            }
        }

        public void Run()
        {

        }

        public void Dispose()
        {
            _isStop = true;
        }
    }
}
