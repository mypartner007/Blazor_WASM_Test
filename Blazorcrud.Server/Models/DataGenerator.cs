using Blazorcrud.Shared.Models;
using Bogus;

namespace Blazorcrud.Server.Models
{
    public class DataGenerator
    {
        public static void Initialize(AppDbContext appDbContext)
        {
            Randomizer.Seed = new Random(32321);
            appDbContext.Database.EnsureDeleted();
            appDbContext.Database.EnsureCreated();
            if (!(appDbContext.Project.Any()))
            {

                // Create new project
                var testProject = new Faker<Blazorcrud.Shared.Models.Project>()
                    .RuleFor(p => p.Name, f => f.Name.JobTitle())
                    .RuleFor(p => p.Description, f => f.Address.StreetName())
                    .RuleFor(p => p.CreatedAt, f => f.Date.Past(1));

                var projects = testProject.Generate(25);

                foreach (Blazorcrud.Shared.Models.Project p in projects)
                {
                    appDbContext.Project.Add(p);
                }
                appDbContext.SaveChanges();
            }
            if (!(appDbContext.Users.Any()))
            {

                var testUsers = new Faker<User>()
                    .RuleFor(u => u.FirstName, u => u.Name.FirstName())
                    .RuleFor(u => u.LastName, u => u.Name.LastName())
                    .RuleFor(u => u.Username, u => u.Internet.UserName())
                    .RuleFor(u => u.Password, u => u.Internet.Password());
                var users = testUsers.Generate(4);

                User customUser = new User(){
                    FirstName = "admin",
                    LastName = "admin",
                    Username = "admin",
                    Password = "admin",
                    IsAdmin = true,
                };

                users.Add(customUser);

                foreach (User u in users)
                {
                    u.PasswordHash = BCrypt.Net.BCrypt.HashPassword("123qweasd");
                    u.Password = "**********";
                    appDbContext.Users.Add(u);
                }
                appDbContext.SaveChanges();
            }
        }
    }
}