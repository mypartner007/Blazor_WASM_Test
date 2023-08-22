using TimeKeep.Server.Authorization;
using TimeKeep.Server.Helpers;
using TimeKeep.Shared.Data;
using TimeKeep.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace TimeKeep.Server.Models
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;
        private IJwtUtils _jwtUtils;

        public UserRepository(AppDbContext appDbContext, IJwtUtils jwtUtils)
        {
            _appDbContext = appDbContext;
            _jwtUtils = jwtUtils;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest request)
        {
            var _user = _appDbContext.Users.SingleOrDefault(u => u.Username == request.Username);

            // validate
            if (_user == null || !BCrypt.Net.BCrypt.Verify(request.Password, _user.PasswordHash))
                throw new AppException("Username or password is incorrect");

            // authentication successful
            AuthenticateResponse response = new AuthenticateResponse();
            response.Id = _user.Id;
            response.LastName = _user.LastName;
            response.FirstName = _user.FirstName;
            response.Username = _user.Username;
            response.IsAdmin = _user.IsAdmin;
            response.Token = _jwtUtils.GenerateToken(_user);
            return response;
        }
        public async Task<User> Register(RegisterRequest request)
        {
            Console.Write(request);
            User user = new()
            {
                Username = request.Username,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = request.Password
            };
            user = await AddUser(user);
            return user;
        }

        public PagedResult<User> GetUsers(string? name, int? page)
        {
            int pageSize = 5;

            if (name != null)
            {
                return _appDbContext.Users
                    .Where(u => u.FirstName.Contains(name, StringComparison.CurrentCultureIgnoreCase) ||
                        u.LastName.Contains(name, StringComparison.CurrentCultureIgnoreCase) || 
                        u.Username.Contains(name, StringComparison.CurrentCultureIgnoreCase))
                    .OrderBy(u => u.Username)
                    .GetPaged((int)page, pageSize);
            }
            else if(page > 0)
            {
                return _appDbContext.Users
                    .OrderBy(u => u.Username)
                    .GetPaged((int)page, pageSize);
            }
            {
                return _appDbContext.Users
                    .OrderBy(u => u.Username)
                    .GetPaged(1, 100);
            }
            
        }

        public async Task<User?> GetUser(int Id)
        {
            var result = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id==Id);
            if (result != null)
            {
                return result;
            }
            else
            {
                throw new KeyNotFoundException("User not found");
            }
        }
        
        public async Task<User> AddUser(User user)
        {
            // validate unique
            if (_appDbContext.Users.Any(u => u.Username == user.Username))
                throw new AppException("Username '" + user.Username + "' is already taken");

            // hash password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            Console.WriteLine(user.Password + " ==> " + user.PasswordHash);
            user.Password = "**********";
            
            var result = await _appDbContext.Users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<User?> UpdateUser(User user)
        {
            var result = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id==user.Id);

            // cannot update admin
            if (result.Username == "admin")
                throw new AppException("Admin may not be updated");

            // validate unique
            if (user.Username != result.Username && _appDbContext.Users.Any(u => u.Username == user.Username))
                throw new AppException("Username '" + user.Username + "' is already taken");

            // hash password if entered
            if(!string.IsNullOrEmpty(user.Password) && user.Password != result.Password)
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
                user.Password = "**********";
            }

            if (result!=null)
            {
                // Update existing user
                _appDbContext.Entry(result).CurrentValues.SetValues(user);
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("User not found");
            }
            return result;
        }

        public async Task<User?> DeleteUser(int Id)
        {
            var result = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id==Id);

            // cannot delete admin
            if (result.Username == "admin")
                throw new AppException("Admin may not be deleted");
                
            if (result!=null)
            {
                _appDbContext.Users.Remove(result);
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("User not found");
            }
            return result;
        }
    }
}