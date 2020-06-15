using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyNote.YoutubeManagement.Models
{
    public class ContentRelationModel
    {
        public Guid ContentId { get; set; }
        public List<Guid> CategoryIds { get; set; }
    }
}
