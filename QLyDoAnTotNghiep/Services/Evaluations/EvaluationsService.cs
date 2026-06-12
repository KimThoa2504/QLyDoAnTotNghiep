using Microsoft.EntityFrameworkCore;
using QLyDoAnTotNghiep.Data;
using QLyDoAnTotNghiep.Models.Evaluations;

namespace QLyDoAnTotNghiep.Services.Evaluations
{
    public class EvaluationsService : IEvaluationsService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public EvaluationsService(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<Evaluation> CreateEvaluationAsync(Evaluation evaluation, List<EvaluationCriterion>? criteria = null)
        {
            evaluation.CreatedAt = DateTime.UtcNow;
            evaluation.EvaluationDate ??= DateTime.UtcNow;

            _context.Evaluations.Add(evaluation);
            await _context.SaveChangesAsync();

            if (criteria != null && criteria.Any())
            {
                foreach (var c in criteria)
                {
                    c.EvaluationId = evaluation.Id;
                    _context.EvaluationCriteria.Add(c);
                }
                await _context.SaveChangesAsync();

                // Tính điểm tổng
                evaluation.TotalScore = CalculateTotalScore(criteria);
                _context.Evaluations.Update(evaluation);
                await _context.SaveChangesAsync();
            }

            return evaluation.GetSafeEvaluation();
        }

        private decimal CalculateTotalScore(IEnumerable<EvaluationCriterion> criteria)
        {
            if (criteria == null || !criteria.Any()) return 0;
            return criteria.Sum(c => c.Score * c.Weight / 100m);
        }

        public async Task<List<Evaluation>> GetAllEvaluationsAsync()
        {
            return await _context.Evaluations
                .Include(e => e.Project)
                .Include(e => e.EvaluationBoard)
                .Include(e => e.CriteriaScores)
                .ToListAsync();
        }

        public async Task<List<Evaluation>> GetEvaluationsByProjectIdAsync(int projectId)
        {
            return await _context.Evaluations
                .Include(e => e.EvaluationBoard)
                .Include(e => e.CriteriaScores)
                .Where(e => e.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<Evaluation?> GetByIdAsync(int id)
        {
            return await _context.Evaluations
                .Include(e => e.Project)
                .Include(e => e.EvaluationBoard)
                .Include(e => e.CriteriaScores)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<bool> UpdateEvaluationAsync(Evaluation evaluation, List<EvaluationCriterion>? criteria = null)
        {
            var existing = await _context.Evaluations
                .Include(e => e.CriteriaScores)
                .FirstOrDefaultAsync(e => e.Id == evaluation.Id);

            if (existing == null) return false;

            // Không cho phép update nếu đã Approved
            if (existing.Status == Evaluation.EvaluationStatus.Approved)
                return false;

            // Update thông tin chính
            existing.EvaluationDate = evaluation.EvaluationDate;
            existing.Session = evaluation.Session;
            existing.Comments = evaluation.Comments;
            existing.Status = evaluation.Status;
            existing.MinutesFilePath = evaluation.MinutesFilePath;
            existing.MinutesPublicId = evaluation.MinutesPublicId;
            existing.UpdatedAt = DateTime.UtcNow;

            // Xử lý Criteria (nếu có)
            if (criteria != null)
            {
                // Xóa criteria cũ
                _context.EvaluationCriteria.RemoveRange(existing.CriteriaScores);

                // Thêm criteria mới
                foreach (var c in criteria)
                {
                    c.EvaluationId = existing.Id;
                    _context.EvaluationCriteria.Add(c);
                }

                // Tính lại điểm
                existing.TotalScore = CalculateTotalScore(criteria);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ApproveEvaluationAsync(int id)
        {
            var evaluation = await _context.Evaluations
                .Include(x => x.CriteriaScores)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (evaluation == null) return false;

            evaluation.TotalScore = CalculateTotalScore(evaluation.CriteriaScores);
            evaluation.Status = Evaluation.EvaluationStatus.Approved;
            evaluation.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectEvaluationAsync(int id)
        {
            var eval = await _context.Evaluations.FindAsync(id);
            if (eval == null) return false;

            eval.Status = Evaluation.EvaluationStatus.Rejected;
            eval.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEvaluationAsync(int id)
        {
            var evaluation = await _context.Evaluations.FindAsync(id);
            if (evaluation == null) return false;

            if (!string.IsNullOrEmpty(evaluation.MinutesFilePath))
            {
                var fullPath = Path.Combine(_environment.WebRootPath, evaluation.MinutesFilePath.TrimStart('/'));
                if (File.Exists(fullPath)) File.Delete(fullPath);
            }

            _context.Evaluations.Remove(evaluation);
            await _context.SaveChangesAsync();
            return true;
        }

        
    }
}