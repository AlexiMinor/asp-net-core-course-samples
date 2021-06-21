using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.CQRS.Queries;

namespace NewsAggregator.DAL.CQRS.QueryHandlers
{
    public class GetUserRoleNameByEmailQueryHandler : IRequestHandler<GetUserRoleNameByEmailQuery, string>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public GetUserRoleNameByEmailQueryHandler(NewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        public async Task<string> Handle(GetUserRoleNameByEmailQuery request, CancellationToken cancellationToken)
        {
            var userRoleId = (await _dbContext.Users.FirstOrDefaultAsync(us => us.Email.Equals(request.Email), cancellationToken: cancellationToken)).RoleId;
            var roleName = (await _dbContext.Roles.FirstOrDefaultAsync(role => role.Id.Equals(userRoleId), cancellationToken: cancellationToken)).Name;
            return roleName;
        }
    }
}