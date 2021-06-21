using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Services.Interfaces;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Implementation;
using Serilog;

namespace NewsAggregators.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public string GetPasswordHash(string modelPassword)
        {
            const string specialValue = "123123123132";
            var sha256 = new SHA256CryptoServiceProvider();
            var sha256data = sha256.ComputeHash(Encoding.UTF8.GetBytes(modelPassword));
            var hashedPassword = Encoding.UTF8.GetString(sha256data);
            return hashedPassword;
        }

        public async Task<bool> RegisterUser(UserDto model)
        {
            try
            {
                var userRoleId =
                    (await _unitOfWork.Roles.FindBy(role => role.Name.Equals("User")).FirstOrDefaultAsync()).Id;
                await _unitOfWork.Users.Add(new User()
                {
                    Id = model.Id,
                    Email = model.Email,
                    PasswordHash = model.PasswordHash,
                    FullName = "",
                    RoleId = userRoleId
                });
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Log.Error(e,"Register was not successful");
                return false;
            }
        }

        public async Task<UserDto> GetUserByEmail(string email)
        {
            try
            {
                var user = await _unitOfWork.Users.FindBy(user => user.Email.Equals(email)).FirstOrDefaultAsync();
                return new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Age = user.Age,
                    PasswordHash = user.PasswordHash,
                    FullName = user.FullName
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<string> GetUserRoleNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CheckAuthIsValid(UserDto model)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetUserEmailByRefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}

