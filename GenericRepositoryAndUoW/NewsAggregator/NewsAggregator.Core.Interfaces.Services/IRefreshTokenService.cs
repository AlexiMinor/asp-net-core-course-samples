using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.DAL.Core.Entities;

namespace NewsAggregator.Core.Services.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<RefreshTokenDto> GenerateRefreshToken(Guid userId);
        Task<bool> CheckIsRefreshTokenIsValid(string requestToken);
    }
}
