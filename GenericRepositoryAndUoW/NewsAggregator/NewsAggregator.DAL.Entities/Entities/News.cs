using System;
using System.Collections.Generic;

namespace NewsAggregator.DAL.Core.Entities
{
    public class News : IBaseEntity
    {
       // [Key]
        public Guid Id /*NewsId*/ { get; set; } // PK
        public string Article { get; set; }
        public string Url { get; set; }
        public string Body { get; set; }
        public string Summary { get; set; }

        public Guid? RssSourseId { get; set; } //FK
        public virtual RssSourse RssSourse { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
