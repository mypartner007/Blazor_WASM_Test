using Blazorcrud.Shared.Data;
using Blazorcrud.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Blazorcrud.Server.Models
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _appDbContext;

        public ProjectRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Project> AddProject(Project project)
        {
            var result = await _appDbContext.Project.AddAsync(project);
            await _appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Project?> DeleteProject(int projectId)
        {
            var result = await _appDbContext.Project.FirstOrDefaultAsync(p => p.Id==projectId);
            if (result!=null)
            {
                _appDbContext.Project.Remove(result);
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Project not found");
            }
            return result;
        }

        public async Task<Project?> GetProject(int id)
        {
            var result = await _appDbContext.Project
                .FirstOrDefaultAsync(p => p.Id == id);
            if (result != null)
            {
                return result;
            }
            else
            {
                throw new KeyNotFoundException("Project not found");
            }
        }

        public PagedResult<Project> GetProjects(string? name, int page)
        {
            int pageSize = 5;
            
            if (name != null)
            {
                return _appDbContext.Project
                    .Where(p => p.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase))
                    .OrderBy(p => p.Id)
                    .GetPaged(page, pageSize);
            }
            else
            {
                return _appDbContext.Project
                    .OrderBy(p => p.Id)
                    .GetPaged(page, pageSize);
            }
        }

        public async Task<Project?> UpdateProject(Project project)
        {
            var result = await _appDbContext.Project.FirstOrDefaultAsync(p => p.Id == project.Id);
            if (result!=null)
            {
                // Update existing project
                _appDbContext.Entry(result).CurrentValues.SetValues(project);
                
              
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Project not found");
            }
            return result;
        }
    }
}