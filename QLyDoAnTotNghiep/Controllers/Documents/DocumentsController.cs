using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLyDoAnTotNghiep.Models.Documents;
using QLyDoAnTotNghiep.Services.Documents;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QLyDoAnTotNghiep.Controllers.Documents
{
    [Route("api/documents")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentsService _documentsService;

        public DocumentsController(IDocumentsService documentsService)
        {
            _documentsService = documentsService;
        }

        [HttpPost("upload")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadDocument(IFormFile file, [FromForm] int projectId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File không hợp lệ");

            try
            {
                var document = await _documentsService.UploadDocumentAsync(file, projectId);
                return Ok(new
                {
                    message = "Upload file thành công",
                    document
                });
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
            var documents = await _documentsService.GetAllDocumentsAsync();
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
            return Ok(documents);
        }

        [HttpGet("project/{projectId}")]
        [Authorize]
        public async Task<IActionResult> GetByProject(int projectId)
        {
            var documents = await _documentsService.GetDocumentsByProjectIdAsync(projectId);
            return Ok(documents);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _documentsService.DeleteDocumentAsync(id);
            if (!result)
                return NotFound("Không tìm thấy tài liệu");

            return Ok(new { message = "Xóa tài liệu thành công" });
        }
    }
}
