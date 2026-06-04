using QLyDoAnTotNghiep.Models.EvaluationBoards;
using QLyDoAnTotNghiep.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QLyDoAnTotNghiep.Models.BoardMembers
{
    public class BoardMember
    {
        [Key]
        public int Id { get; set; }

        [Column("board_id")]
        public int BoardId { get; set; }

        [ForeignKey("BoardId")]
        public EvaluationBoard? EvaluationBoard { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Column("role")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BoardRole Role { get; set; } = BoardRole.Member;

        public enum BoardRole
        {
            Chairman,
            Secretary,
            Member
        }

        public BoardMember GetSafeBoardMember()
        {
            return new BoardMember
            {
                Id = this.Id,
                BoardId = this.BoardId,
                UserId = this.UserId,
                Role = this.Role
            };
        }
    }
}
