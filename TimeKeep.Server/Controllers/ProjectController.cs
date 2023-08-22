using TimeKeep.Server.Authorization;
using TimeKeep.Server.Models;
using TimeKeep.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace TimeKeep.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        /// <summary>
        /// Returns a list of paginated projects with a default page size of 5.
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetProjects([FromQuery] int? assigneeId, int page)
        {
            return Ok(_projectRepository.GetProjects(assigneeId, page));
        }

        /// <summary>
        /// Gets a specific project by Id.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetProject(int id)
        {
            return Ok(await _projectRepository.GetProject(id));
        }

        /// <summary>
        /// Creates a project with child addresses.
        /// </summary>
        [AllowAnonymous]
        /// 
        [HttpPost]
        public async Task<ActionResult> AddProject(Project project)
        {
            return Ok(await _projectRepository.AddProject(project));
        }
        
        /// <summary>
        /// Updates a project with a specific Id.
        /// </summary>
        [HttpPut]
        public async Task<ActionResult> UpdateProject(Project project)
        {
            return Ok(await _projectRepository.UpdateProject(project));
        }

        /// <summary>
        /// Deletes a project with a specific Id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProject(int id)
        {
            return Ok(await _projectRepository.DeleteProject(id));
        }
    }
}
