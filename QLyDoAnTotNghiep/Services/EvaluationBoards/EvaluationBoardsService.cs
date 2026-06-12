using QLyDoAnTotNghiep.Data;
using Microsoft.EntityFrameworkCore;
using QLyDoAnTotNghiep.Models.EvaluationBoards;

namespace QLyDoAnTotNghiep.Services.EvaluationBoards
{
    public class EvaluationBoardsService : IEvaluationBoardsService
    {
        private readonly AppDbContext _context;

        public EvaluationBoardsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EvaluationBoard> CreateEvaluationBoardAsync(EvaluationBoard board)
        {
            if (await _context.EvaluationBoards.AnyAsync(b => b.Name == board.Name))
                throw new Exception("Tên hội đồng đã tồn tại");

            board.CreatedAt = DateTime.UtcNow;
            _context.EvaluationBoards.Add(board);
            await _context.SaveChangesAsync();
            return board.GetSafeEvaluationBoard();
        }

        public async Task<List<EvaluationBoard>> GetAllEvaluationBoardsAsync()
        {
            return await _context.EvaluationBoards
                .Include(b => b.BoardMembers)
                    .ThenInclude(m => m.User)
                .Select(b => b.GetSafeEvaluationBoard())
                .ToListAsync();
        }
        public async Task<List<EvaluationBoard>> GetActiveBoardsAsync()
        {
            return await _context.EvaluationBoards
                .Where(b => b.Status == EvaluationBoard.BoardStatus.Active)
                .ToListAsync();
        }

        public async Task<EvaluationBoard?> GetByIdAsync(int id)
        {
            return await _context.EvaluationBoards
                .Include(b => b.BoardMembers)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<bool> UpdateEvaluationBoardAsync(EvaluationBoard board)
        {
            var existing = await _context.EvaluationBoards.FindAsync(board.Id);
            if (existing == null) return false;

            existing.Name = board.Name;
            existing.Description = board.Description;
            existing.Type = board.Type;
            existing.Status = board.Status;
            existing.FormedDate = board.FormedDate;
            existing.ExpiredDate = board.ExpiredDate;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEvaluationBoardAsync(int id)
        {
            var board = await _context.EvaluationBoards.FindAsync(id);
            if (board == null) return false;

            _context.EvaluationBoards.Remove(board);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignProjectToBoardAsync(int boardId, int projectId)
        {
            var exists = await _context.Evaluations.AnyAsync(e =>
                e.BoardId == boardId && e.ProjectId == projectId);

            if (exists)
                throw new Exception("Đề tài này đã được giao cho hội đồng");

            return true;
        }
    }
}
