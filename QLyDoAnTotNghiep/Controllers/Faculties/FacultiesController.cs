using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLyDoAnTotNghiep.Models.Faculties;
using QLyDoAnTotNghiep.Services.Faculties;

namespace QLyDoAnTotNghiep.Controllers.Faculties
{
    [Route("api/faculties")]
    [ApiController]
    public class FacultiesController : ControllerBase
    {
        private readonly IFacultiesService _facultiesService;

        public FacultiesController(IFacultiesService facultiesService)
        {
            _facultiesService = facultiesService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var faculties = await _facultiesService.GetAllFacultiesAsync();
            return Ok(faculties);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var faculty = await _facultiesService.GetFacultyByIdAsync(id);
            if (faculty == null) return NotFound();
            return Ok(faculty);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] Faculty faculty)
        {
            try
            {
                var created = await _facultiesService.CreateFacultyAsync(faculty);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] Faculty faculty)
        {
            faculty.Id = id;
            var success = await _facultiesService.UpdateFacultyAsync(faculty);
            if (!success) return NotFound();
            return Ok(new { message = "Cập nhật khoa thành công" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _facultiesService.DeleteFacultyAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Xóa khoa thành công" });
        }
    }
}
