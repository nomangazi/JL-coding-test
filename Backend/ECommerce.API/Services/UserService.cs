using ECommerce.Core.DTOs;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;
using ECommerce.Core.Services;

namespace ECommerce.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(MapToDto).ToList();
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user != null ? MapToDto(user) : null;
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user != null ? MapToDto(user) : null;
        }

        public async Task<UserDto> CreateUserAsync(UserCreateRequest request)
        {
            // Validate unique email
            var existingUser = await _userRepository.ExistsByEmailAsync(request.Email);
            if (existingUser)
            {
                throw new Exception("Email already exists");
            }

            var user = new User
            {
                Name = request.Name.Trim(),
                Email = request.Email.ToLower().Trim(),
                Phone = request.Phone.Trim(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdUser = await _userRepository.CreateAsync(user);
            return MapToDto(createdUser);
        }

        public async Task<UserDto> UpdateUserAsync(int id, UserUpdateRequest request)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Check if email is being updated and if it's unique
            if (!string.IsNullOrEmpty(request.Email) && request.Email != user.Email)
            {
                var emailExists = await _userRepository.ExistsByEmailAsync(request.Email, id);
                if (emailExists)
                {
                    throw new Exception("Email already exists");
                }
                user.Email = request.Email.ToLower().Trim();
            }

            // Update other fields only if provided
            if (!string.IsNullOrEmpty(request.Name))
                user.Name = request.Name.Trim();

            if (!string.IsNullOrEmpty(request.Phone))
                user.Phone = request.Phone.Trim();

            user.UpdatedAt = DateTime.UtcNow;

            var updatedUser = await _userRepository.UpdateAsync(user);
            return MapToDto(updatedUser);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }

        private static UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }
    }
}