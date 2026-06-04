using QLyDoAnTotNghiep.Models.Faculties;

namespace QLyDoAnTotNghiep.Services.Faculties
{
    public interface IFacultiesService
    {
        Task<Faculty> CreateFacultyAsync(Faculty faculty);
        Task<List<Faculty>> GetAllFacultiesAsync();
        Task<Faculty?> GetFacultyByIdAsync(int id);
        Task<bool> UpdateFacultyAsync(Faculty faculty);
        Task<bool> DeleteFacultyAsync(int id);
    }
}
