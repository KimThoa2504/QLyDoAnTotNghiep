using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLyDoAnTotNghiep.Models.Projects;
using QLyDoAnTotNghiep.Services.Projects;

namespace QLyDoAnTotNghiep.Controllers.Projects
{
    [Route("api/projects")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectsService _projectsService;

        public ProjectsController(IProjectsService projectsService)
        {
            _projectsService = projectsService;
        }

        //Get
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var projects = await _projectsService.GetAllProjectsAsync();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var project = await _projectsService.GetProjectByIdAsync(id);
            if (project == null) return NotFound();
            return Ok(project);
        }

        //Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Project project)
        {
            try
            {
                var created = await _projectsService.CreateProjectAsync(project);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //update
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Project project)
        {
            project.Id = id;
            var success = await _projectsService.UpdateProjectAsync(project);
            if (!success) return NotFound();
            return Ok(new { message = "Cập nhật đề tài thành công" });
        }

        //delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _projectsService.DeleteProjectAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Xóa đề tài thành công" });
        }
    }
}
