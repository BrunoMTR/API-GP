using Domain.DTOs;
using Domain.Repositories;
using Infrastructure.SQL.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SQL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DemoContext _demoContext;
        public UserRepository(DemoContext demoContext)
        {
            _demoContext = demoContext;
        }

        public Task<UserDto> CreateAsync(UserDto user)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDto?> RetrieveAsync(string username)
        {
            var userEntity = await _demoContext.User.FirstOrDefaultAsync(u => u.Username == username);

            if (userEntity == null)
                return null;

            return new UserDto
            {
                Id = userEntity.Id,
                Username = userEntity.Username,
                Password = userEntity.Password,
                Role = userEntity.Role,
                IsActive = userEntity.IsActive,
                LastChanged = userEntity.LastChanged
            };
        }
    }
}
