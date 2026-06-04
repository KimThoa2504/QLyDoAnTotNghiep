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

            _context.EvaluationBoards.Add(board);
            await _context.SaveChangesAsync();
            return board.GetSafeEvaluationBoard();
        }

        public async Task<List<EvaluationBoard>> GetAllEvaluationBoardsAsync()
        {
            return await _context.EvaluationBoards
                .Select(b => new EvaluationBoard
                {
                    Id = b.Id,
                    Name = b.Name,
                    Description = b.Description
                })
                .ToListAsync();
        }

        public async Task<EvaluationBoard?> GetByIdAsync(int id)
        {
            return await _context.EvaluationBoards.FindAsync(id);
        }

        public async Task<bool> UpdateEvaluationBoardAsync(EvaluationBoard board)
        {
            var existing = await _context.EvaluationBoards.FindAsync(board.Id);
            if (existing == null) return false;

            existing.Name = board.Name;
            existing.Description = board.Description;

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
    }
}
