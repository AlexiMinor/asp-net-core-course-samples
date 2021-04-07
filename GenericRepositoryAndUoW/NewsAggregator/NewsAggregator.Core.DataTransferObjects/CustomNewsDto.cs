using System;

namespace NewsAggregator.Core.DataTransferObjects
{
    public class CustomNewsDto //Data transfer objects
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Text { get; set; }
        public string ShortDescription { get; set; }
        public string CustomSpecificValue { get; set; }

        public Guid? RssSourseId { get; set; }
    }
}
