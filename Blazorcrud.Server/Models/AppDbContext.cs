using Blazorcrud.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace Blazorcrud.Server.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Project> Project => Set<Project>();
        public DbSet<User> Users => Set<User>();
    }
}


