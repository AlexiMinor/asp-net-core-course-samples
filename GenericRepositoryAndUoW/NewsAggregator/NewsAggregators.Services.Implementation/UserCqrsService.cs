using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Services.Interfaces;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.CQRS.Commands;
using NewsAggregator.DAL.CQRS.Queries;
using Serilog;

namespace NewsAggregators.Services.Implementation
{
    public class UserCqrsService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public UserCqrsService(IMapper mapper, IMediator mediator, IConfiguration configuration)
        {
            _mapper = mapper;
            _mediator = mediator;
            _configuration = configuration;
        }


        public string GetPasswordHash(string modelPassword)
        {
            var specialValue = _configuration["Password:SecuritySymmetricKey"];
            var sha256 = new SHA256CryptoServiceProvider();
            var sha256data = sha256.ComputeHash(Encoding.UTF8.GetBytes(modelPassword));
            var hashedPassword = Encoding.UTF8.GetString(sha256data);
            return hashedPassword;
        }

        public async Task<bool> RegisterUser(UserDto model)
        {
            try
            {
                var user = await _mediator.Send(new RegisterUserCommand()
                {
                    Email = model.Email,
                    PasswordHash = model.PasswordHash
                });
                return true;
            }
            catch (Exception e)
            {
                Log.Error(e,"Register was not successful");
                return false;
            }
        }

        public async Task<bool> CheckAuthIsValid(UserDto model)
        {
            try
            {
                var user = await _mediator.Send(new CheckAuthenticationQuery()
                {
                    Email = model.Email,
                    PasswordHash = model.PasswordHash
                });
                return true;
            }
            catch (Exception e)
            {
                Log.Error(e, "Register was not successful");
                return false;
            }
        }

        public async Task<string> GetUserEmailByRefreshToken(string refreshToken)
        {
            try
            {
                var userEmail = await _mediator.Send(new GetUserEmailByRefreshTokenQuery()
                {
                    Token = refreshToken
                });

                return userEmail;
            }
            catch (Exception e)
            {
                Log.Error(e, "Refresh token was not successful");
                throw;
            }
        }

        public async Task<UserDto> GetUserByEmail(string email)
        {
            try
            {
                var user = await _mediator.Send(new GetUserByEmailQuery() {Email = email});
                return user;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                throw;
            }
        }

        public async Task<string> GetUserRoleNameByEmail(string email)
        {
            try
            {
                var roleName = await _mediator.Send(new GetUserRoleNameByEmailQuery { Email = email });
                return roleName;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                throw;
            }
        }
    }
}