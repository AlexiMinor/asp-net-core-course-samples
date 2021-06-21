using System;
using MediatR;

namespace NewsAggregator.DAL.CQRS.Commands
{
    public class RegisterUserCommand : IRequest<int>
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}