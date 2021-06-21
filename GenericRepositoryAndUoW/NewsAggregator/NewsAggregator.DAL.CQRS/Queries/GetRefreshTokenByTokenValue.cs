using MediatR;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.DAL.CQRS.Queries
{
    public class GetRefreshTokenByTokenValueQuery : IRequest<RefreshTokenDto>
    {
        public string TokenValue { get; set; }
    }
}