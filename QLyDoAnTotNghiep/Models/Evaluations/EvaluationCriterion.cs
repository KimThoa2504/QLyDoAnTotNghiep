using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLyDoAnTotNghiep.Models.Evaluations
{
    public class EvaluationCriterion
    {
        [Key]
        public int Id { get; set; }

        [Column("evaluation_id")]
        public int EvaluationId { get; set; }
        public Evaluation? Evaluation { get; set; }

        [Required]
        [Column("criterion_name")]
        public string CriterionName { get; set; } = string.Empty;

        [Column("weight")]
        public decimal Weight { get; set; }   // Tỉ lệ %

        [Column("score")]
        public decimal Score { get; set; }    // Điểm đạt

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

