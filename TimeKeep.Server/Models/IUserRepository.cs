using TimeKeep.Server.Authorization;
using TimeKeep.Shared.Data;
using TimeKeep.Shared.Models;

namespace TimeKeep.Server.Models
{
    public interface IUserRepository
    {
        AuthenticateResponse Authenticate(AuthenticateRequest request);
        Task<User> Register(RegisterRequest request);
        PagedResult<User> GetUsers(string? name, int? page);
        Task<User?> GetUser(int Id);
        Task<User> AddUser(User user);
        Task<User?> UpdateUser(User user);
        Task<User?> DeleteUser(int id);
    }
}