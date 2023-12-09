using SmartHouseHub.Domain.Identity;
using SmartHouseHub.Domain.Response;
using SmartHouseHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouseHub.Service.Interfaces
{
    public interface IUserService
    {
        Task<BaseResponse<User>> Register(UserRequestModel model);

    }
}
