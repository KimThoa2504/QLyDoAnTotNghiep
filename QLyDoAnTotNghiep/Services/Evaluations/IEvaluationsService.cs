using QLyDoAnTotNghiep.Models.Evaluations;

namespace QLyDoAnTotNghiep.Services.Evaluations
{
    public interface IEvaluationsService
    {
        Task<Evaluation> CreateEvaluationAsync(Evaluation evaluation);
        Task<List<Evaluation>> GetAllEvaluationsAsync();
        Task<List<Evaluation>> GetEvaluationsByProjectIdAsync(int projectId);
        Task<Evaluation?> GetByIdAsync(int id);
        Task<bool> UpdateEvaluationAsync(Evaluation evaluation);
        Task<bool> DeleteEvaluationAsync(int id);
    }
}
