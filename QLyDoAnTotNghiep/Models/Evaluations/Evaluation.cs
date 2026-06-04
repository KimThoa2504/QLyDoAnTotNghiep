using QLyDoAnTotNghiep.Models.EvaluationBoards;
using QLyDoAnTotNghiep.Models.Projects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public string? Comments { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? Score { get; set; }

        public Evaluation GetSafeEvaluation()
        {
            return new Evaluation
            {
                Id = this.Id,
                ProjectId = this.ProjectId,
                BoardId = this.BoardId,
                EvaluationDate = this.EvaluationDate,
                Comments = this.Comments,
                Score = this.Score
            };
        }
    }
}
