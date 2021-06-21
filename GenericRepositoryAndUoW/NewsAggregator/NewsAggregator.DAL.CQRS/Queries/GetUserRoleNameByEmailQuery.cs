using MediatR;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.DAL.CQRS.Queries
{
    public class GetUserRoleNameByEmailQuery : IRequest<string>
    {
        public string Email { get; set; }
    }
}