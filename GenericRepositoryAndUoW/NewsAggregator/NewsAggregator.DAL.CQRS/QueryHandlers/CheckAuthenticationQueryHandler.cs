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
    public class CheckAuthenticationQueryHandler : IRequestHandler<CheckAuthenticationQuery, bool>
    {
        private readonly NewsAggregatorContext _dbContext;

        public CheckAuthenticationQueryHandler(NewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<bool> Handle(CheckAuthenticationQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Users.AnyAsync(
                u => u.Email.Equals(request.Email) && u.PasswordHash.Equals(request.PasswordHash),
                cancellationToken: cancellationToken);
        }
    }
}