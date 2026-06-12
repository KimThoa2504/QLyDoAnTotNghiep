using QLyDoAnTotNghiep.Models.EvaluationBoards;

namespace QLyDoAnTotNghiep.Services.EvaluationBoards
{
    public interface IEvaluationBoardsService
    {
        Task<EvaluationBoard> CreateEvaluationBoardAsync(EvaluationBoard board);
        Task<List<EvaluationBoard>> GetAllEvaluationBoardsAsync();
        Task<EvaluationBoard?> GetByIdAsync(int id);
        Task<bool> UpdateEvaluationBoardAsync(EvaluationBoard board);
        Task<bool> DeleteEvaluationBoardAsync(int id);
        Task<List<EvaluationBoard>> GetActiveBoardsAsync();
        Task<bool> AssignProjectToBoardAsync(int boardId, int projectId);
    }
}
