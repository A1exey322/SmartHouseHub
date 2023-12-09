using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmartHouseHub.DAL.Interfaces;
using SmartHouseHub.Domain.Enums;
using SmartHouseHub.Domain.Identity;
using SmartHouseHub.Domain.Response;
using SmartHouseHub.Models;
using SmartHouseHub.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouseHub.Service.Implementations
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IBaseRepository<User> _userrepository;
        private readonly JWTSettings _options;
        private readonly ILogger<TokenProvider> _logger;

        public TokenProvider(IOptions<JWTSettings> optAccess, ILogger<TokenProvider> logger, IBaseRepository<User> userrepository)
        {
            _options = optAccess.Value;
            _logger = logger;
            _userrepository = userrepository;
        }

        public async Task<BaseResponse<string>> CreateToken(UserRequestModel request)
        {

            try
            {
                var user_ = await _userrepository.GetAll().FirstOrDefaultAsync(x => x.Username == request.Username);

                if (user_ == null)
                {
                    return new BaseResponse<string>()
                    {
                        Description = $"User with username '{request.Username}' is not found"
                    };
                }

                if (!BCrypt.Net.BCrypt.Verify(request.Password, user_.PasswordHash))
                {
                    return new BaseResponse<string>()
                    {
                        Description = $"Wrong password!"
                    };
                }


                List<Claim> claims = new List<Claim>
                {
                new Claim(ClaimTypes.Name, user_.Username),
                new Claim("level", "123"),
                new Claim(ClaimTypes.Role, "Admin")
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var jwt = new JwtSecurityToken(
                    issuer: _options.Issuer,
                    audience: _options.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

                string jwttoken = new JwtSecurityTokenHandler().WriteToken(jwt);

                return new BaseResponse<string>()
                {
                    Data = jwttoken,
                    StatusCode = StatusCode.OK,
                    Description = "Token is created!"

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[CreateToken]: {ex.Message}");
                return new BaseResponse<string>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }


        }
    }
}
