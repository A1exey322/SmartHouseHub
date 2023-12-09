using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartHouseHub.DAL.Interfaces;
using SmartHouseHub.Domain.Enums;
using SmartHouseHub.Domain.Identity;
using SmartHouseHub.Domain.Response;
using SmartHouseHub.Models;
using SmartHouseHub.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouseHub.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User> _userrepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IBaseRepository<User> userrepository, ILogger<UserService> logger)
        {
            _userrepository = userrepository;
            _logger = logger;
        }   

        public async Task<BaseResponse<User>> Register(UserRequestModel model)
        {
            try
            {
                var user_ = await _userrepository.GetAll().FirstOrDefaultAsync(x => x.Username == model.Username);

                if (user_ != null)
                {
                    return new BaseResponse<User>()
                    {
                        Description = $"User with username '{model.Username}' is exist",
                    };

                }

                string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

                User user = new User
                {
                    Username = model.Username,
                    PasswordHash = passwordHash
                };

                await _userrepository.Create(user);

                return new BaseResponse<User>()
                {
                    Data = user,
                    Description = "Объект добавился",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[Register]: {ex.Message}");
                return new BaseResponse<User>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }




    }
}
