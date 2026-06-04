using QLyDoAnTotNghiep.Models.Projects;

namespace QLyDoAnTotNghiep.Services.Projects
{
    public interface IProjectsService
    {
        Task<Project> CreateProjectAsync(Project project);
        Task<List<Project>> GetAllProjectsAsync();
        Task<Project?> GetProjectByIdAsync(int id);
        Task<bool> UpdateProjectAsync(Project project);
        Task<bool> DeleteProjectAsync(int id);
    }
}
