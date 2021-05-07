using System;

namespace NewsAggregator.Core.DataTransferObjects
{
    public class CommentDto //Data transfer objects
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
        public Guid NewsId { get; set; } //FK
        public Guid UserId { get; set; } //FK
    }
}
