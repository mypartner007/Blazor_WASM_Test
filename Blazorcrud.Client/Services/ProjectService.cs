using Blazorcrud.Client.Shared;
using Blazorcrud.Shared.Data;
using Blazorcrud.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Blazorcrud.Client.Services
{
    public class ProjectService: IProjectService
    {
        private IHttpService _httpService;

        public ProjectService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<PagedResult<Project>> GetProjects(string? name, string page)
        {
            return await _httpService.Get<PagedResult<Project>>("api/project" + "?page=" + page + "&name=" + name);
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