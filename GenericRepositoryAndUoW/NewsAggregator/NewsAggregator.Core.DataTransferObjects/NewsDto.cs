using System;

namespace NewsAggregator.Core.DataTransferObjects
{
    public class NewsDto //Data transfer objects
    {
        public Guid Id { get; set; }
        public string Article { get; set; }
        public string Url { get; set; }
        public string Body { get; set; }
        public string Summary { get; set; }

        public Guid? RssSourseId { get; set; }
    }
}
