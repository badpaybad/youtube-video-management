using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;

namespace MoneyNote.Identity.Enities.Extensions
{
    public static class CommmonEntitiesExtensions
    {
        static HttpClient _httpClient = new HttpClient();

        static Stream GetFile(string urlOfPath)
        {
            if (string.IsNullOrEmpty(urlOfPath)) return null;

            if(urlOfPath.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                || urlOfPath.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                return _httpClient.GetStreamAsync(urlOfPath).GetAwaiter().GetResult();
            }

            return File.OpenRead(urlOfPath);
        }

        public static CmsContent CalculateThumbnail(this CmsContent content)
        {
            if (content.ThumbnailWidth != 0 && content.ThumbnailHeight != 0) return content;

            var f = GetFile(content.Thumbnail);
            if (f == null || f == Stream.Null)
            {
                return content;
            }

            var bmp = new Bitmap(f);

            content.ThumbnailWidth = bmp.Width;
            content.ThumbnailHeight = bmp.Height;

            return content;
        }
    }
}
