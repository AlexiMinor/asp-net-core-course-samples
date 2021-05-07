using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.Models.ViewModels.News
{
    public class NewsWithCommentsViewModel
    {
        public Guid Id { get; set; }
        public string Article { get; set; }
        public string Url { get; set; }
        public string Body { get; set; }
        public string Summary { get; set; }
        public Guid? RssSourseId { get; set; }
        public string RssSourseName { get; set; }

        public IEnumerable<CommentDto> Comments { get; set; }
    }
}
