using QLyDoAnTotNghiep.Models.EvaluationBoards;
using QLyDoAnTotNghiep.Models.Projects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QLyDoAnTotNghiep.Models.Evaluations
{
    public class Evaluation
    {
        [Key]
        public int Id { get; set; }

        [Column("project_id")]
        public int ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public Project? Project { get; set; }

        [Column("board_id")]
        public int BoardId { get; set; }

        [ForeignKey("BoardId")]
        public EvaluationBoard? EvaluationBoard { get; set; }

        [Column("evaluation_date")]
        public DateTime? EvaluationDate { get; set; }

        [Column("session")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EvaluationSession Session { get; set; } = EvaluationSession.Final;

        [Column("total_score")]
        public decimal? TotalScore { get; set; }

        public string? Comments { get; set; }

        [Column("status")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EvaluationStatus Status { get; set; } = EvaluationStatus.Pending;

        // File biên bản
        [Column("minutes_file_path")]
        public string? MinutesFilePath { get; set; }
        [Column("minutes_public_id")]
        public string? MinutesPublicId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        public ICollection<EvaluationCriterion> CriteriaScores { get; set; } = new List<EvaluationCriterion>();

        public enum EvaluationSession { Midterm, Final, ReDefense, Other }
        public enum EvaluationStatus { Pending, Approved, Rejected }

        public Evaluation GetSafeEvaluation()
        {
            return new Evaluation
            {
                Id = this.Id,
                ProjectId = this.ProjectId,
                BoardId = this.BoardId,
                EvaluationDate = this.EvaluationDate,
                Session = this.Session,
                TotalScore = this.TotalScore,
                Comments = this.Comments,
                Status = this.Status,
                MinutesFilePath = this.MinutesFilePath,
                CreatedAt = this.CreatedAt
            };
        }
    }
}
