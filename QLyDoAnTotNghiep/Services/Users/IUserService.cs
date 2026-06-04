using QLyDoAnTotNghiep.Models.Users;

namespace QLyDoAnTotNghiep.Services.Users
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(User user);
        Task<User?> GetUserByIdAsync(int id);
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> UpdateUserAsync(User user);
        string GenerateJwtToken(User user);
        Task<User?> AuthenticateAsync(string username, string password);
        Task<User> CreateAdminAsync(string username, string password, string fullName, string email);
    }
}
