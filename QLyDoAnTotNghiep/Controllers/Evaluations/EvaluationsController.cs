using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLyDoAnTotNghiep.Data;
using QLyDoAnTotNghiep.Models.Evaluations;
using QLyDoAnTotNghiep.Services.Evaluations;

namespace QLyDoAnTotNghiep.Controllers.Evaluations
{
    [Route("api/evaluations")]
    [ApiController]
    [Authorize]
    public class EvaluationsController : ControllerBase
    {
        private readonly IEvaluationsService _evaluationsService;
        private readonly AppDbContext _context;

        public EvaluationsController(AppDbContext context, IEvaluationsService evaluationsService)
        {
            _evaluationsService = evaluationsService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var evaluations = await _evaluationsService.GetAllEvaluationsAsync();
            return Ok(evaluations);
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetByProject(int projectId)
        {
            var evaluations = await _evaluationsService.GetEvaluationsByProjectIdAsync(projectId);
            return Ok(evaluations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var evaluation = await _evaluationsService.GetByIdAsync(id);
            if (evaluation == null) return NotFound();
            return Ok(evaluation);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Lecturer")]
        public async Task<IActionResult> Create([FromForm] EvaluationCreateRequest request)
        {
            try
            {
                var evaluation = new Evaluation
                {
                    ProjectId = request.ProjectId,
                    BoardId = request.BoardId,
                    EvaluationDate = request.EvaluationDate,
                    Session = request.Session,
                    Comments = request.Comments,
                    Status = Evaluation.EvaluationStatus.Pending
                };

                var created = await _evaluationsService.CreateEvaluationAsync(evaluation, request.Criteria);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] EvaluationUpdateRequest request)
        {
            var evaluation = new Evaluation
            {
                Id = id,
                EvaluationDate = request.EvaluationDate,
                Session = request.Session,
                Comments = request.Comments,
                Status = request.Status
            };

            var success = await _evaluationsService.UpdateEvaluationAsync(evaluation, request.Criteria);
            if (!success)
                return BadRequest(new { message = "Không thể cập nhật (đã duyệt hoặc không tồn tại)" });

            return Ok(new { message = "Cập nhật đánh giá thành công" });
        }

        [HttpPost("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(int id)
        {
            var success = await _evaluationsService.ApproveEvaluationAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Đánh giá đã được phê duyệt" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _evaluationsService.DeleteEvaluationAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Xóa đánh giá thành công" });
        }
    }

    // DTO cho Create
    public class EvaluationCreateRequest
    {
        public int ProjectId { get; set; }
        public int BoardId { get; set; }
        public DateTime? EvaluationDate { get; set; }
        public Evaluation.EvaluationSession Session { get; set; } = Evaluation.EvaluationSession.Final;
        public string? Comments { get; set; }
        public List<EvaluationCriterion>? Criteria { get; set; }
    }

    // DTO cho Update (hỗ trợ Criteria)
    public class EvaluationUpdateRequest
    {
        public DateTime? EvaluationDate { get; set; }
        public Evaluation.EvaluationSession Session { get; set; }
        public string? Comments { get; set; }
        public Evaluation.EvaluationStatus Status { get; set; }
        public List<EvaluationCriterion>? Criteria { get; set; }
    }
}