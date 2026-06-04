using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLyDoAnTotNghiep.Models.ProjectMembers;
using QLyDoAnTotNghiep.Services.ProjectMembers;

namespace QLyDoAnTotNghiep.Controllers.ProjectMembers
{
    [Route("api/projectmembers")]
    [ApiController]
    public class ProjectMembersController : ControllerBase
    {
        private readonly IProjectMembersService _projectMembersService;

        public ProjectMembersController(IProjectMembersService projectMembersService)
        {
            _projectMembersService = projectMembersService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var members = await _projectMembersService.GetAllProjectMembersAsync();
            return Ok(members);
        }

        [HttpGet("project/{projectId}")]
        [Authorize]
        public async Task<IActionResult> GetByProjectId(int projectId)
        {
            var members = await _projectMembersService.GetMembersByProjectIdAsync(projectId);
            return Ok(members);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] ProjectMember member)
        {
            try
            {
                var created = await _projectMembersService.CreateProjectMemberAsync(member);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var member = await _projectMembersService.GetByIdAsync(id);
            if (member == null) return NotFound();
            return Ok(member);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] ProjectMember member)
        {
            member.Id = id;
            var success = await _projectMembersService.UpdateProjectMemberAsync(member);
            if (!success) return NotFound();
            return Ok(new { message = "Cập nhật thành viên thành công" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _projectMembersService.DeleteProjectMemberAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Xóa thành viên thành công" });
        }

    }
}
