using TimeKeep.Shared.Data;
using TimeKeep.Shared.Models;
using Bogus;
using Bogus.DataSets;
using Microsoft.EntityFrameworkCore;
using System;

namespace TimeKeep.Server.Models
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

        public Task<Project?> GetProject(int id)
        {
            var result = _appDbContext.Project.Where(c => c.Id == id)
                         .Include(a => a.Assignee)
                         .Include(a2 => a2.Tracks)
                         .SingleOrDefault();

            if (result != null)
            {
                return Task.FromResult<Project?>(result);
            }
            else
            {
                throw new KeyNotFoundException("Project not found");
            }
        }

        public PagedResult<Project> GetProjects(int? assigneeId, int page)
        {
            int pageSize = 5;
            
            if (assigneeId > 0)
            {
                return _appDbContext.Project
                    .Where(p => p.AssigneeId == assigneeId)
                    .OrderBy(p => p.Id)
                    .Include(p => p.Assignee)
                    .Include(a2 => a2.Tracks)
                    .GetPaged(page, pageSize);
            }
            else
            {
                return _appDbContext.Project
                    .OrderBy(p => p.Id)
                    .Include(p => p.Assignee)
                    .Include(a2 => a2.Tracks)
                    .GetPaged(page, pageSize);
            }
        }

        public async Task<Project?> UpdateProject(Project project)
        {
            var result = _appDbContext.Project.Where(c => c.Id == project.Id)
                         .Include(a => a.Assignee)
                         .Include(a2 => a2.Tracks)
                         .SingleOrDefault();
            if (result!=null)
            {
                // Update existing project
                _appDbContext.Entry(result).CurrentValues.SetValues(project);
                // Remove deleted addresses
                if (result.Tracks != null) 
                { 
                    foreach (var existingTrack in result.Tracks.ToList())
                    {
                        if (!project.Tracks.Any(o => o.Id == existingTrack.Id))
                            _appDbContext.Tracks.Remove(existingTrack);
                    }
                }
                // Update and Insert Addresses
                if (project.Tracks != null)
                {
                    foreach (var track in project.Tracks)
                    {
                        var existingTrack = result.Tracks
                            .Where(a => a.Id == track.Id)
                            .SingleOrDefault();
                        if (existingTrack != null)
                            _appDbContext.Entry(existingTrack).CurrentValues.SetValues(track);
                        else
                        {
                            var newTrack = new Track
                            {
                                Date = track.Date,
                                Hours = track.Hours,
                                Receipt = track.Receipt,
                            };
                            result.Tracks.Add(track);
                        }
                    }
                }
                
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