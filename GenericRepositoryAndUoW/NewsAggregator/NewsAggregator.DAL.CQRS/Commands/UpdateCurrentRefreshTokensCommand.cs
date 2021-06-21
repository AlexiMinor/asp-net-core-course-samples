using System;
using MediatR;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.DAL.CQRS.Commands
{
    public class UpdateCurrentRefreshTokensCommand : IRequest<int>
    {
        public Guid UserId { get; set; }

        public RefreshTokenDto NewRefreshToken { get; set; }
    }
}