using Blazorcrud.Shared.Data;
using Blazorcrud.Shared.Models;

namespace Blazorcrud.Server.Models
{
    public interface IProjectRepository
    {
        PagedResult<Project> GetProjects(string? name, int page);
        Task<Project?> GetProject(int id);
        Task<Project> AddProject(Project project);
        Task<Project?> UpdateProject(Project project);
        Task<Project?> DeleteProject(int id);
    }
}