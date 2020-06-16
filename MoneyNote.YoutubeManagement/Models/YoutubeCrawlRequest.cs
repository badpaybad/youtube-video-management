using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyNote.YoutubeManagement.Models
{
    public class YoutubeCrawlRequest
    {
        public string url { get; set; }

        public bool autoSave { get; set; } = false;
    }
        
}
