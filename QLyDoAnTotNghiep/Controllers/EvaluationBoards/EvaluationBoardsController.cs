using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLyDoAnTotNghiep.Models.EvaluationBoards;
using QLyDoAnTotNghiep.Services.EvaluationBoards;

namespace QLyDoAnTotNghiep.Controllers.EvaluationBoards
{
    [Route("api/evaluationboards")]
    [ApiController]
    public class EvaluationBoardsController : ControllerBase
    {
        private readonly IEvaluationBoardsService _evaluationBoardsService;

        public EvaluationBoardsController(IEvaluationBoardsService evaluationBoardsService)
        {
            _evaluationBoardsService = evaluationBoardsService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var boards = await _evaluationBoardsService.GetAllEvaluationBoardsAsync();
            return Ok(boards);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var board = await _evaluationBoardsService.GetByIdAsync(id);
            if (board == null) return NotFound();
            return Ok(board);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EvaluationBoard board)
        {
            try
            {
                var created = await _evaluationBoardsService.CreateEvaluationBoardAsync(board);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EvaluationBoard board)
        {
            board.Id = id;
            var success = await _evaluationBoardsService.UpdateEvaluationBoardAsync(board);
            if (!success) return NotFound();
            return Ok(new { message = "Cập nhật hội đồng thành công" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _evaluationBoardsService.DeleteEvaluationBoardAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Xóa hội đồng thành công" });
        }
    }
}
