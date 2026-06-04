using QLyDoAnTotNghiep.Models.ProjectMembers;

namespace QLyDoAnTotNghiep.Services.ProjectMembers
{
    public interface IProjectMembersService
    {
        Task<ProjectMember> CreateProjectMemberAsync(ProjectMember member);
        Task<List<ProjectMember>> GetAllProjectMembersAsync();
        Task<List<ProjectMember>> GetMembersByProjectIdAsync(int projectId);
        Task<ProjectMember?> GetByIdAsync(int id);
        Task<bool> UpdateProjectMemberAsync(ProjectMember member);
        Task<bool> DeleteProjectMemberAsync(int id);
    }
}
