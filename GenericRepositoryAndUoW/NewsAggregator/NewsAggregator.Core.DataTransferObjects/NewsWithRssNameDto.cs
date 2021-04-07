using System;

namespace NewsAggregator.Core.DataTransferObjects
{
    public class NewsWithRssNameDto 
    {
        public Guid Id { get; set; }
        public string Article { get; set; }
        public string Url { get; set; }
        public string Body { get; set; }
        public string Summary { get; set; }
        public Guid? RssSourseId { get; set; }
        public string RssSourseName { get; set; }
    }
}
