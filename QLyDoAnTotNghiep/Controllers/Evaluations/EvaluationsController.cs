using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLyDoAnTotNghiep.Models.Evaluations;
using QLyDoAnTotNghiep.Services.Evaluations;

namespace QLyDoAnTotNghiep.Controllers.Evaluations
{
    [Route("api/evaluations")]
    [ApiController]
    public class EvaluationsController : ControllerBase
    {
        private readonly IEvaluationsService _evaluationsService;

        public EvaluationsController(IEvaluationsService evaluationsService)
        {
            _evaluationsService = evaluationsService;
        }

        //Get
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var evaluations = await _evaluationsService.GetAllEvaluationsAsync();
            return Ok(evaluations);
        }

        [HttpGet("project/{projectId}")]
        [Authorize]
        public async Task<IActionResult> GetByProjectId(int projectId)
        {
            var evaluations = await _evaluationsService.GetEvaluationsByProjectIdAsync(projectId);
            return Ok(evaluations);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var evaluation = await _evaluationsService.GetByIdAsync(id);
            if (evaluation == null) return NotFound();
            return Ok(evaluation);
        }

        //Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] Evaluation evaluation)
        {
            try
            {
                var created = await _evaluationsService.CreateEvaluationAsync(evaluation);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //update
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] Evaluation evaluation)
        {
            evaluation.Id = id;
            var success = await _evaluationsService.UpdateEvaluationAsync(evaluation);
            if (!success) return NotFound();
            return Ok(new { message = "Cập nhật đánh giá thành công" });
        }

        //delete
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _evaluationsService.DeleteEvaluationAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Xóa đánh giá thành công" });
        }
    }
}
