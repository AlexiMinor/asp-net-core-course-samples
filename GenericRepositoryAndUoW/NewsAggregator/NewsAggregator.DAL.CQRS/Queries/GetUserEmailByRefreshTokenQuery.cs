using MediatR;

namespace NewsAggregator.DAL.CQRS.Queries
{
    public class GetUserEmailByRefreshTokenQuery : IRequest<string>
    {
        public string Token { get; set; }
    }
}