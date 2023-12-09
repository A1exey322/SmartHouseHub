using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouseHub.Domain.Enums
{
    public enum StatusCode
    {
        UserNotFound = 0,
        UserAlreadyExists = 1,
        
        NotFound = 10,

        OK = 200,

        InternalServerError = 500
    }
}
