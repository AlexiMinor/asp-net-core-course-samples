using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.Entities;

namespace NewsAggregator.DAL.Repositories.Implementation.Repositories
{
    public class CommentsRepository : Repository<Comment>
    {
        public CommentsRepository(NewsAggregatorContext context) 
            : base(context)
        {
        }
    }
}