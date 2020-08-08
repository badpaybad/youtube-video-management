using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MoneyNote.YoutubeManagement.Repository;

namespace MoneyNote.YoutubeManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //new YoutubeCrawler().CrawlChannel("https://www.youtube.com/channel/UCfGn2JDKc4faJw29XiC95PQ/videos");

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
