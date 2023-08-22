using TimeKeep.Client.Shared;
using TimeKeep.Shared.Data;
using TimeKeep.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace TimeKeep.Client.Services
{
    public class ProjectService: IProjectService
    {
        private IHttpService _httpService;

        public ProjectService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<PagedResult<Project>> GetProjects(int? assigneeId, string page)
        {
            return await _httpService.Get<PagedResult<Project>>("api/project" + "?assigneeId=" + assigneeId + "&page=" + page);
        }

        public async Task<Project> GetProject(int id)
        {
            return await _httpService.Get<Project>($"api/project/{id}");
        }

        public async Task DeleteProject(int id)
        {
            await _httpService.Delete($"api/project/{id}");
        }

        public async Task AddProject(Project project)
        {
            await _httpService.Post($"api/project", project);
        }

        public async Task UpdateProject(Project project)
        {
            await _httpService.Put($"api/project", project);
        }
    }
}