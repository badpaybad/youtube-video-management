using MoneyNote.Core.Schedules;
using MoneyNote.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyNote.YoutubeManagement.Repository
{
    public class YoutubeCrawlerScheduler : SchedulerAbstract
    {
        public YoutubeCrawlerScheduler() : base(new SchedulerOption
        {
            RunFirstTime = true,
            RunType = SchedulerOption.ScheduleRunType.Daily
        ,
            HourOfDay = 1,
            MinuteOfHour = 1
        })
        {
        }

        YoutubeCrawler _crawler = new YoutubeCrawler();

        public override void JobToDo()
        {
            var map = GetMapping();

            foreach (var m in map)
            {
                var vidsInChanl = _crawler.CrawlChannel(m.Key);
                var cateId = Guid.Parse(m.Value);
                Task.Run(() =>
                {
                    try
                    {
                        using (var db = new MoneyNoteDbContext())
                        {
                            db.ChangeTracker.AutoDetectChangesEnabled = false;

                            foreach (var v in vidsInChanl)
                            {
                                var exited = db.CmsContents.FirstOrDefault(i => i.Title == v.Title);
                                if (exited == null)
                                {
                                    db.CmsContents.Add(v);

                                    db.CmsRelations.Add(new Identity.Enities.CmsRelation
                                    {
                                        ContentId = v.Id,
                                        CategoryId = cateId
                                    });
                                }

                            }
                            db.SaveChanges();
                        }


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                });
            }
        }

        private List<KeyValuePair<string, string>> GetMapping()
        {
            List<KeyValuePair<string, string>> map = new List<KeyValuePair<string, string>>();
            using (var sr = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "youtubeschedule.ini")))
            {
                var all = sr.ReadToEnd().Split('\n');
                foreach (var l in all)
                {
                    var arr = l.Split(new[] { '|', ';' });
                    if (arr.Length > 1)
                    {
                        map.Add(new KeyValuePair<string, string>(arr[0].Trim(), arr[1].Trim()));
                    }
                }
            }

            return map;
        }
    }
}
