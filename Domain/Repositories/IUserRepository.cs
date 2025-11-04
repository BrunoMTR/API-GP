using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        Task<UserDto?> RetrieveAsync(string username);
        Task<UserDto> CreateAsync(UserDto user);
    }
}
