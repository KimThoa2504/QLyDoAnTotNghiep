using QLyDoAnTotNghiep.Models.Reports;

namespace QLyDoAnTotNghiep.Services.Reports
{
    public interface IReportsService
    {
        Task<Report> GenerateReportAsync(string reportType, string description, IFormFile? file = null);
        Task<List<Report>> GetAllReportsAsync();
        Task<bool> DeleteReportAsync(long id);
    }
}
