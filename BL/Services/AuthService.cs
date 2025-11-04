using BL.Utils;
using Domain.DTOs;
using Domain.Repositories;
using Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;

        public AuthService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }
        public async Task<UserDto?> AuthenticateUserAsync(string username, string password)
        {
            var user = await _userRepo.RetrieveAsync(username);
            if (user == null || !AuthUtil.VerifyPassword(password, user.Password) || !user.IsActive)
            {
                return null;
            }
            return user;
        }
    }
}
