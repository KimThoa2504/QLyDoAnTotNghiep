using QLyDoAnTotNghiep.Models.Documents;

namespace QLyDoAnTotNghiep.Services.Documents
{
    public interface IDocumentsService
    {
        Task<Document> UploadDocumentAsync(IFormFile file, int projectId);
        Task<List<Document>> GetAllDocumentsAsync();
        Task<List<Document>> GetDocumentsByProjectIdAsync(int projectId);
        Task<Document?> GetByIdAsync(long id);
        Task<bool> DeleteDocumentAsync(long id);
    }
}
