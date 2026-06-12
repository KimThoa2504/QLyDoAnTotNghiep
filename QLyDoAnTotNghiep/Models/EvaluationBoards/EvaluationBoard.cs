using QLyDoAnTotNghiep.Models.BoardMembers;
using QLyDoAnTotNghiep.Models.Evaluations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

        [Column("type")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BoardType Type { get; set; } = BoardType.Defense;

        [Column("status")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BoardStatus Status { get; set; } = BoardStatus.Active;

        [Column("formed_date")]
        public DateTime? FormedDate { get; set; }

        [Column("expired_date")]
        public DateTime? ExpiredDate { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        public ICollection<BoardMember> BoardMembers { get; set; } = new List<BoardMember>();
        public ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();

        public enum BoardType
        {
            Defense, Midterm, Final, Review, Other
        }

        public enum BoardStatus
        {
            Active, Completed, Cancelled
        }

        public EvaluationBoard GetSafeEvaluationBoard()
        {
            return new EvaluationBoard
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                Type = this.Type,
                Status = this.Status,
                FormedDate = this.FormedDate,
                ExpiredDate = this.ExpiredDate,
                CreatedAt = this.CreatedAt
            };
        }
    }
}
