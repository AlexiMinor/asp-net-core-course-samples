using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.CQRS.Commands;

namespace NewsAggregator.DAL.CQRS.CommandHandlers
{
    class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, int>
    {
        private readonly NewsAggregatorContext _dbContext;

        public RegisterUserCommandHandler(NewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var userRoleId = (await _dbContext.Roles.FirstOrDefaultAsync(role => role.Name.Equals("User"), cancellationToken: cancellationToken)).Id;

            await _dbContext.Users.AddAsync(new User()
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                PasswordHash = request.PasswordHash,
                FullName = "",
                RoleId = userRoleId
            }, cancellationToken);

            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
