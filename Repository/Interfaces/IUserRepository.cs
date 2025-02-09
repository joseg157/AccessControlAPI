using server.Models;

namespace server.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsernameAsync(string username);

        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);

        Task<User> AddUserAsync(User user);

        Task<User> UpdateUserAsync(User user);
    }
}
