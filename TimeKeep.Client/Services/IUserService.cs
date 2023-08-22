using TimeKeep.Client.Shared;
using TimeKeep.Shared.Data;
using TimeKeep.Shared.Models;

namespace TimeKeep.Client.Services
{
    public interface IUserService
    {
        User User {get; }
        Task Initialize();
        Task Register(Register model);
        Task Login(Login model);
        Task Logout();
        Task<PagedResult<User>> GetUsers(string? name, string? page);
        Task<User> GetUser(int id);
        Task DeleteUser(int id);
        Task AddUser(User user);
        Task UpdateUser(User user);
    }
}