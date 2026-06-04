using QLyDoAnTotNghiep.Data;
using Microsoft.EntityFrameworkCore;
using QLyDoAnTotNghiep.Models.Projects;

namespace QLyDoAnTotNghiep.Services.Projects
{
    public class ProjectsService : IProjectsService
    {
        private readonly AppDbContext _context;

        public ProjectsService(AppDbContext context)
        {
            _context = context;
        }

        //Create
        public async Task<Project> CreateProjectAsync(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project.GetSafeProject();
        }

        //Get
        public async Task<List<Project>> GetAllProjectsAsync()
        {
            return await _context.Projects
                .Include(p => p.Faculty)
                .Select(p => new Project
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    Status = p.Status,
                    FacultyId = p.FacultyId
                })
                .ToListAsync();
        }

        public async Task<Project?> GetProjectByIdAsync(int id)
        {
            return await _context.Projects
                .Include(p => p.Faculty)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        //Update
        public async Task<bool> UpdateProjectAsync(Project project)
        {
            var existing = await _context.Projects.FindAsync(project.Id);
            if (existing == null) return false;

            existing.Name = project.Name;
            existing.Description = project.Description;
            existing.StartDate = project.StartDate;
            existing.EndDate = project.EndDate;
            existing.Status = project.Status;
            existing.FacultyId = project.FacultyId;

            await _context.SaveChangesAsync();
            return true;
        }

        //delete
        public async Task<bool> DeleteProjectAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null) return false;

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
