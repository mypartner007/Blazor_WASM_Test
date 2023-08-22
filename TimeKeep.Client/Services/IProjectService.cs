using TimeKeep.Shared.Data;
using TimeKeep.Shared.Models;

namespace TimeKeep.Client.Services
{
    public interface IProjectService
    {
        Task<PagedResult<Project>> GetProjects(int? assigneeId, string page );
        Task<Project> GetProject(int id);

        Task DeleteProject(int id);

        Task AddProject(Project project);

        Task UpdateProject(Project project);
    }
}