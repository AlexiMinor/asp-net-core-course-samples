using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.CQRS.Queries;

namespace NewsAggregator.DAL.CQRS.QueryHandlers
{
    public class GetUserEmailByRefreshTokenQueryHandler : IRequestHandler<GetUserEmailByRefreshTokenQuery, string>
    {
        private readonly NewsAggregatorContext _dbContext;

        public GetUserEmailByRefreshTokenQueryHandler(NewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<string> Handle(GetUserEmailByRefreshTokenQuery request, CancellationToken cancellationToken)
        {
            var refreshTokenUserId =
                (await _dbContext.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(rt => rt.Token.Equals(request.Token), cancellationToken: cancellationToken)).UserId;

            return (await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(
                u => u.Id.Equals(refreshTokenUserId),
                cancellationToken: cancellationToken)).Email;
        }
    }
}