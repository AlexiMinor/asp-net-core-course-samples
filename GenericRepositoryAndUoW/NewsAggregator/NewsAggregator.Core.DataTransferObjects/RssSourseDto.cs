using System;

namespace NewsAggregator.Core.DataTransferObjects
{
    public class RssSourseDto //Data transfer objects
    {
        public Guid Id { get; set; } //16B
        public string Name { get; set; } //up to 20B
        public string Url { get; set; } //up to 1KB
    }
}
