using TimeKeep.Shared.Data;
using TimeKeep.Shared.Models;

namespace TimeKeep.Server.Models
{
    public interface IProjectRepository
    {
        PagedResult<Project> GetProjects(int? assigneeId, int page);
        Task<Project?> GetProject(int id);
        Task<Project> AddProject(Project project);
        Task<Project?> UpdateProject(Project project);
        Task<Project?> DeleteProject(int id);
    }
}