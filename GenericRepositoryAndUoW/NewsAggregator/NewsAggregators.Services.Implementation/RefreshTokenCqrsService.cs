using System;
using System.Threading.Tasks;
using MediatR;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Services.Interfaces;
using NewsAggregator.DAL.CQRS.Commands;
using NewsAggregator.DAL.CQRS.Queries;

namespace NewsAggregators.Services.Implementation
{
    public class RefreshTokenCqrsService : IRefreshTokenService
    {
        private readonly IMediator _mediator;

        public RefreshTokenCqrsService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<RefreshTokenDto> GenerateRefreshToken(Guid userId)
        {
            var newRefreshToken = new RefreshTokenDto()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Token = Guid.NewGuid().ToString(),
                CreationDate = DateTime.Now.ToUniversalTime(),
                ExpiresUtc = DateTime.Now.AddHours(1)
            };

            await _mediator.Send(new UpdateCurrentRefreshTokensCommand
            {
                UserId = userId,
                NewRefreshToken = newRefreshToken
            });

            return newRefreshToken;
        }

        public async Task<bool> CheckIsRefreshTokenIsValid(string refreshToken)
        {
            var rt = await _mediator.Send(new GetRefreshTokenByTokenValueQuery()
                {TokenValue = refreshToken});

            return rt != null && rt.ExpiresUtc >= DateTime.Now;
        }
    }
}