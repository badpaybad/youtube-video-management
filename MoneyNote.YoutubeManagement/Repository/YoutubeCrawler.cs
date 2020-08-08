using MoneyNote.Core.Schedules;
using MoneyNote.Identity;
using MoneyNote.Identity.Enities;
using MoneyNote.Identity.Enities.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

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
    public class YoutubeChanenlInfo
    {
        public contentChannel contents { get; set; }

        public class contentChannel
        {
            public twoColumnBrowseResultsRenderer twoColumnBrowseResultsRenderer { get; set; }
        }

        public class twoColumnBrowseResultsRenderer
        {
            public List<tabRenderer> tabs { get; set; }
        }

        public class tabRenderer
        {
            public tabContents content { get; set; }
            public class tabContents
            {
                public sectionListRenderer sectionListRenderer { get; set; }

            }

            public class sectionListRenderer
            {
                List<itemSectionRenderer> contents { get; set; }

                public class itemSectionRenderer
                {
                    public List<gridRenderer> contents { get; set; }
                    public class gridRenderer
                    {

                        public List<gridVideoRenderer> items { get; set; }
                        public class gridVideoRenderer
                        {
                            public string videoId { get; set; }

                        }

                    }
                }

            }
        }
    }
    public class YoutubeCrawler
    {

        public List<CmsContent> CrawlChannel(string url)
        {
            var channelContent = string.Empty;
            using (var httpClient = new HttpClient())
            {

                httpClient.BaseAddress = new Uri("https://www.youtube.com/");
                var result = httpClient.GetStringAsync(url).GetAwaiter().GetResult();

                channelContent = FindYoutubeContent(result, "window[\"ytInitialData\"] =", "window[\"ytInitialPlayerResponse\"] =");
            }
            //channelContent = channelContent.Trim(new[] { ' ', ';' });
            //var objChannel = System.Text.Json.JsonSerializer.Deserialize<YoutubeChanenlInfo>(channelContent);
            //window["ytInitialData"]
            //window["ytInitialPlayerResponse"]

            var matchesVideoId = System.Text.RegularExpressions.Regex.Matches(channelContent, "\"videoId\":[\\\" \\w]+\"");
            List<string> videosIds = new List<string>();
            foreach (Match m in matchesVideoId)
            {
                var v = m.Value.Split(':').LastOrDefault();
                if (!string.IsNullOrEmpty(v)) videosIds.Add(v.Trim(new[] { ' ', '"' }));
            }

            videosIds = videosIds.Distinct().ToList();

            List<CmsContent> results = new List<CmsContent>();

            Parallel.ForEach(videosIds, new ParallelOptions { MaxDegreeOfParallelism = 12 }
            , (vid) =>
            {

                var url = $"https://www.youtube.com/watch?v={vid}";

                var c = CrawlVideo(url, out Exception ex);
                if (ex == null) results.Add(c);

            });

            return results;
        }
        public CmsContent CrawlVideo(string url, out Exception ex)
        {
            ex = null;
            try
            {
                using (var httpClient = new HttpClient())
                {

                    httpClient.BaseAddress = new Uri("https://www.youtube.com/");
                    string result;

                    result = httpClient.GetStringAsync(url).GetAwaiter().GetResult();

                    int.TryParse(FindYoutubeContent(result, "<meta property=\"og:image:width\" content=\"", "\">"), out int tw);
                    int.TryParse(FindYoutubeContent(result, "<meta property=\"og:image:height\" content=\"", "\">"), out int th);
                    int.TryParse(FindYoutubeContent(result, "<meta property=\"og:video:width\" content=\"", "\">"), out int vw);
                    int.TryParse(FindYoutubeContent(result, "<meta property=\"og:video:height\" content=\"", "\">"), out int vh);

                    CmsContent cmsContent = new CmsContent
                    {
                        Title = FindYoutubeContent(result, "<meta property=\"og:title\" content=\"", "\">"),
                        Thumbnail = FindYoutubeContent(result, "<meta property=\"og:image\" content=\"", "\">"),
                        UrlRef = FindYoutubeContent(result, "<meta property=\"og:url\" content=\"", "\">"),
                        Description = FindYoutubeContent(result, "\\\"description\\\":{\\\"simpleText\\\":\\\"", "\\\"}"),
                        ThumbnailHeight = th,
                        ThumbnailWidth = tw,
                        VideoHeight = vh,
                        VideoWidth = vw,
                    };

                    cmsContent = cmsContent.CalculateThumbnail();

                    return cmsContent;
                }

            }
            catch (Exception ex1)
            {
                ex = ex1;
                return null;
            }

        }

        string FindYoutubeContent(string src, string begin, string end)
        {
            var idx = src.IndexOf(begin);
            if (idx < 0) return string.Empty;

            var temp = src.Substring(idx + begin.Length);
            idx = temp.IndexOf(end);

            var data = temp.Substring(0, idx).Replace("\\\\n", " ").Replace("\\n", " ").Replace("\\\\", " ").Replace("\\", " ");
            data = data.Trim(new char[] { ' ', '\\', '/', '"', '\r', '\n' });
            return data;
        }
    }
}
