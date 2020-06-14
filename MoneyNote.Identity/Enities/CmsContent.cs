using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyNote.Identity.Enities
{
    [Table("CmsContent")]
    public class CmsContent : AbastractEntity
    {
        public string Title { get; set; }
        public string Thumbnail { get; set; }
        public string Description { get; set; }
        public string UrlRef { get; set; }

        public long CountView { get; set; }
    }
}
