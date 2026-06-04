using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLyDoAnTotNghiep.Services.Reports;

namespace QLyDoAnTotNghiep.Controllers.Reports
{
    [Route("api/reports")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportsService _reportsService;

        public ReportsController(IReportsService reportsService)
        {
            _reportsService = reportsService;
        }

        [HttpPost("generate")]
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> GenerateReport([FromForm] string reportType, [FromForm] string description, IFormFile? file)
        {
            try
            {
                var report = await _reportsService.GenerateReportAsync(reportType, description, file);
                return Ok(new { message = "Tạo báo cáo thành công", report });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var reports = await _reportsService.GetAllReportsAsync();
            return Ok(reports);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(long id)
        {
            var success = await _reportsService.DeleteReportAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Xóa báo cáo thành công" });
        }
    }
}
