using QLyDoAnTotNghiep.Data;
using Microsoft.EntityFrameworkCore;
using QLyDoAnTotNghiep.Models.Reports;

namespace QLyDoAnTotNghiep.Services.Reports
{
    public class ReportsService : IReportsService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ReportsService(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<Report> GenerateReportAsync(string reportType, string description, IFormFile? file = null)
        {
            string? filePath = null;

            if (file != null && file.Length > 0)
            {
                var reportsFolder = Path.Combine(_environment.WebRootPath, "uploads", "reports");
                if (!Directory.Exists(reportsFolder))
                    Directory.CreateDirectory(reportsFolder);

                var uniqueName = $"{Guid.NewGuid()}_{file.FileName}";
                var fullPath = Path.Combine(reportsFolder, uniqueName);
                filePath = $"/uploads/reports/{uniqueName}";

                using var stream = new FileStream(fullPath, FileMode.Create);
                await file.CopyToAsync(stream);
            }

            var report = new Report
            {
                ReportType = reportType,
                Description = description,
                FilePath = filePath,
                GeneratedAt = DateTime.UtcNow
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            return report.GetSafeReport();
        }

        public async Task<List<Report>> GetAllReportsAsync()
        {
            return await _context.Reports.ToListAsync();
        }

        public async Task<bool> DeleteReportAsync(long id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null) return false;

            if (!string.IsNullOrEmpty(report.FilePath))
            {
                var fullPath = Path.Combine(_environment.WebRootPath, report.FilePath.TrimStart('/'));
                if (File.Exists(fullPath)) File.Delete(fullPath);
            }

            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
