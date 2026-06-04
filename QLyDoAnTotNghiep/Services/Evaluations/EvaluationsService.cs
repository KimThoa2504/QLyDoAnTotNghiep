using QLyDoAnTotNghiep.Data;
using Microsoft.EntityFrameworkCore;
using QLyDoAnTotNghiep.Models.Evaluations;

namespace QLyDoAnTotNghiep.Services.Evaluations
{
    public class EvaluationsService : IEvaluationsService
    {
        private readonly AppDbContext _context;

        public EvaluationsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Evaluation> CreateEvaluationAsync(Evaluation evaluation)
        {
            _context.Evaluations.Add(evaluation);
            await _context.SaveChangesAsync();
            return evaluation.GetSafeEvaluation();
        }

        //Get
        public async Task<List<Evaluation>> GetAllEvaluationsAsync()
        {
            return await _context.Evaluations
                .Include(e => e.Project)
                .Include(e => e.EvaluationBoard)
                .Select(e => new Evaluation
                {
                    Id = e.Id,
                    ProjectId = e.ProjectId,
                    BoardId = e.BoardId,
                    EvaluationDate = e.EvaluationDate,
                    Comments = e.Comments,
                    Score = e.Score
                })
                .ToListAsync();
        }
        public async Task<List<Evaluation>> GetEvaluationsByProjectIdAsync(int projectId)
        {
            return await _context.Evaluations
                .Include(e => e.EvaluationBoard)
                .Where(e => e.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<Evaluation?> GetByIdAsync(int id)
        {
            return await _context.Evaluations
                .Include(e => e.Project)
                .Include(e => e.EvaluationBoard)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<bool> UpdateEvaluationAsync(Evaluation evaluation)
        {
            var existing = await _context.Evaluations.FindAsync(evaluation.Id);
            if (existing == null) return false;

            existing.ProjectId = evaluation.ProjectId;
            existing.BoardId = evaluation.BoardId;
            existing.EvaluationDate = evaluation.EvaluationDate;
            existing.Comments = evaluation.Comments;
            existing.Score = evaluation.Score;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEvaluationAsync(int id)
        {
            var evaluation = await _context.Evaluations.FindAsync(id);
            if (evaluation == null) return false;

            _context.Evaluations.Remove(evaluation);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
