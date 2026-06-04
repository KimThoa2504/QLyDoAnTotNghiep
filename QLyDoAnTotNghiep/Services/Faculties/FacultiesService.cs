using QLyDoAnTotNghiep.Data;
using Microsoft.EntityFrameworkCore;
using QLyDoAnTotNghiep.Models.Faculties;

namespace QLyDoAnTotNghiep.Services.Faculties
{
    public class FacultiesService : IFacultiesService
    {
        private readonly AppDbContext _context;

        public FacultiesService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Faculty> CreateFacultyAsync(Faculty faculty)
        {
            if (await _context.Faculties.AnyAsync(f => f.Name == faculty.Name))
                throw new Exception("Tên khoa đã tồn tại");

            _context.Faculties.Add(faculty);
            await _context.SaveChangesAsync();
            return faculty.GetSafeFaculty();
        }

        public async Task<List<Faculty>> GetAllFacultiesAsync()
        {
            return await _context.Faculties
                .Select(f => new Faculty
                {
                    Id = f.Id,
                    Name = f.Name,
                    Description = f.Description
                })
                .ToListAsync();
        }

        public async Task<Faculty?> GetFacultyByIdAsync(int id)
        {
            return await _context.Faculties.FindAsync(id);
        }

        public async Task<bool> UpdateFacultyAsync(Faculty faculty)
        {
            var existing = await _context.Faculties.FindAsync(faculty.Id);
            if (existing == null) return false;

            existing.Name = faculty.Name;
            existing.Description = faculty.Description;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteFacultyAsync(int id)
        {
            var faculty = await _context.Faculties.FindAsync(id);
            if (faculty == null) return false;

            _context.Faculties.Remove(faculty);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
