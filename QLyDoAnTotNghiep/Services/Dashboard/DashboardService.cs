using Microsoft.EntityFrameworkCore;
using QLyDoAnTotNghiep.Data;
using QLyDoAnTotNghiep.Models.Users;

namespace QLyDoAnTotNghiep.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<object> GetDashboardStatsAsync()
        {
            var totalProjects = await _context.Projects.CountAsync();
            var totalUsers = await _context.Users.CountAsync();
            var totalLecturers = await _context.Users.CountAsync(u => u.Role == User.UserRole.Lecturer);
            var totalFaculties = await _context.Faculties.CountAsync();
            var totalDocuments = await _context.Documents.CountAsync();

            var projectsByStatus = await _context.Projects
                .GroupBy(p => p.Status.ToString())
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Status, x => x.Count);

            var projectsByFaculty = await _context.Projects
                .Where(p => p.Faculty != null)
                .GroupBy(p => p.Faculty!.Name)
                .Select(g => new { Faculty = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Faculty, x => x.Count);

            var recentProjects = await _context.Projects
                .Include(p => p.Faculty)
                .OrderByDescending(p => p.Id)
                .Take(5)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    Status = p.Status.ToString(),
                    FacultyName = p.Faculty != null ? p.Faculty.Name : "Không thuộc khoa",
                    p.StartDate
                })
                .ToListAsync();

            return new
            {
                TotalProjects = totalProjects,
                TotalUsers = totalUsers,
                TotalLecturers = totalLecturers,
                TotalFaculties = totalFaculties,
                TotalDocuments = totalDocuments,
                ProjectsByStatus = projectsByStatus,
                ProjectsByFaculty = projectsByFaculty,
                RecentProjects = recentProjects
            };
        }
    }
}
