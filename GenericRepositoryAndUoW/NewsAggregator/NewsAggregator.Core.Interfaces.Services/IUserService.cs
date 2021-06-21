using System.Threading.Tasks;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.Core.Services.Interfaces
{
    public interface IUserService
    {
        string GetPasswordHash(string modelPassword);
        Task<bool> RegisterUser(UserDto model);
        Task<UserDto> GetUserByEmail(string email);
        Task<string> GetUserRoleNameByEmail(string email);
        Task<bool> CheckAuthIsValid(UserDto model);
        Task<string> GetUserEmailByRefreshToken(string refreshToken);
    }
}