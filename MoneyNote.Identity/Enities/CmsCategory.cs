using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyNote.Identity.Enities
{
    [Table("cmscategory")]
    public class CmsCategory: AbastractEntity
    {
        public string Title { get; set; }
    }
}
