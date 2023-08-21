using Blazorcrud.Shared.Data;
using Blazorcrud.Shared.Models;

namespace Blazorcrud.Client.Services
{
    public interface IProjectService
    {
        Task<PagedResult<Project>> GetProjects(string? name, string page);
        Task<Project> GetProject(int id);

        Task DeleteProject(int id);

        Task AddProject(Project project);

        Task UpdateProject(Project project);
    }
}