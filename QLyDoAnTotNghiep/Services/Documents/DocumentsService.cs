using Microsoft.EntityFrameworkCore;
using QLyDoAnTotNghiep.Data;

using QLyDoAnTotNghiep.Models.Documents;

namespace QLyDoAnTotNghiep.Services.Documents
{
    public class DocumentsService : IDocumentsService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public DocumentsService(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<Document> UploadDocumentAsync(IFormFile file, int projectId)
        {
            if (file == null || file.Length == 0)
                throw new Exception("File không hợp lệ");

            // Tạo thư mục
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "Documents", projectId.ToString());
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);
            var projectExists = await _context.Projects.AnyAsync(p => p.Id == projectId);
            if (!projectExists)
                throw new Exception($"Project với ID {projectId} không tồn tại.");

            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var fullPath = Path.Combine(uploadsFolder, uniqueFileName);
            var relativePath = $"/uploads/Documents/{projectId}/{uniqueFileName}";

            // Lưu file
            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            var document = new Document
            {
                ProjectId = projectId,
                FileName = file.FileName,
                FilePath = relativePath,
                FileSize = file.Length,
                FileType = Path.GetExtension(file.FileName).ToLowerInvariant(),
                UploadedAt = DateTime.UtcNow
            };

            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            return document.GetSafeDocument();
        }

        public async Task<List<Document>> GetAllDocumentsAsync()
        {
            return await _context.Documents
                .Include(d => d.Project)
                .ToListAsync();
        }

        public async Task<List<Document>> GetDocumentsByProjectIdAsync(int projectId)
        {
            return await _context.Documents
                .Where(d => d.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<Document?> GetByIdAsync(long id)
        {
            return await _context.Documents.FindAsync(id);
        }

        public async Task<bool> DeleteDocumentAsync(long id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document == null) return false;

            // Xóa file vật lý
            var fullPath = Path.Combine(_environment.WebRootPath, document.FilePath.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
