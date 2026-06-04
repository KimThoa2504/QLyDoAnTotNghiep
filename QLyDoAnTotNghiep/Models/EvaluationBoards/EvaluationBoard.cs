using System.ComponentModel.DataAnnotations;

namespace QLyDoAnTotNghiep.Models.EvaluationBoards
{
    public class EvaluationBoard
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public EvaluationBoard GetSafeEvaluationBoard()
        {
            return new EvaluationBoard
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description
            };
        }
    }
}
