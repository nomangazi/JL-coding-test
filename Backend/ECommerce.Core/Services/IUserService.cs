using ECommerce.Core.DTOs;
using ECommerce.Core.Entities;

namespace ECommerce.Core.Services
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<UserDto?> GetUserByEmailAsync(string email);
        Task<UserDto> CreateUserAsync(UserCreateRequest request);
        Task<UserDto> UpdateUserAsync(int id, UserUpdateRequest request);
        Task DeleteUserAsync(int id);
    }
}