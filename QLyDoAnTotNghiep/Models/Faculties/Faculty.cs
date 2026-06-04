using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLyDoAnTotNghiep.Models.Faculties
{
    public class Faculty
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Column("description")]
        public string? Description { get; set; }

        public Faculty GetSafeFaculty()
        {
            return new Faculty
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description
            };
        }
    }
}
