using MediatR;

namespace NewsAggregator.DAL.CQRS.Queries
{
    public class CheckAuthenticationQuery : IRequest<bool>
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}