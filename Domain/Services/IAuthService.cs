using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IAuthService
    {
        Task<UserDto?> AuthenticateUserAsync(string username, string password);
    }
}
