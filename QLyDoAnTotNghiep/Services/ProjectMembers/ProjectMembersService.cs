using Microsoft.EntityFrameworkCore;
using QLyDoAnTotNghiep.Data;
using QLyDoAnTotNghiep.Models.ProjectMembers;

namespace QLyDoAnTotNghiep.Services.ProjectMembers
{
    public class ProjectMembersService : IProjectMembersService
    {
        private readonly AppDbContext _context;

        public ProjectMembersService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProjectMember> CreateProjectMemberAsync(ProjectMember member)
        {
            _context.ProjectMembers.Add(member);
            await _context.SaveChangesAsync();
            return member.GetSafeProjectMember();
        }

        public async Task<List<ProjectMember>> GetAllProjectMembersAsync()
        {
            return await _context.ProjectMembers
                .Include(pm => pm.Project)
                .Select(pm => new ProjectMember
                {
                    Id = pm.Id,
                    ProjectId = pm.ProjectId,
                    MaSinhVien = pm.MaSinhVien,
                    HoVaTen = pm.HoVaTen,
                    Role = pm.Role
                })
                .ToListAsync();
        }

        public async Task<List<ProjectMember>> GetMembersByProjectIdAsync(int projectId)
        {
            return await _context.ProjectMembers
                .Include(pm => pm.Project)
                .Where(pm => pm.ProjectId == projectId)
                .Select(pm => new ProjectMember
                {
                    Id = pm.Id,
                    ProjectId = pm.ProjectId,
                    MaSinhVien = pm.MaSinhVien,
                    HoVaTen = pm.HoVaTen,
                    Role = pm.Role
                })
                .ToListAsync();
        }

        public async Task<ProjectMember?> GetByIdAsync(int id)
        {
            return await _context.ProjectMembers.FindAsync(id);
        }

        public async Task<bool> UpdateProjectMemberAsync(ProjectMember member)
        {
            var existing = await _context.ProjectMembers.FindAsync(member.Id);
            if (existing == null) return false;

            existing.MaSinhVien = member.MaSinhVien;
            existing.HoVaTen = member.HoVaTen;
            existing.Role = member.Role;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProjectMemberAsync(int id)
        {
            var member = await _context.ProjectMembers.FindAsync(id);
            if (member == null) return false;

            _context.ProjectMembers.Remove(member);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
