using QLyDoAnTotNghiep.Models.Documents;
using QLyDoAnTotNghiep.Models.Faculties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QLyDoAnTotNghiep.Models.Projects
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Column("start_date")]
        public DateTime? StartDate { get; set; }

        [Column("end_date")]
        public DateTime? EndDate { get; set; }

        [Column("status")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ProjectStatus Status { get; set; } = ProjectStatus.Pending;

        [Column("faculty_id")]
        public int? FacultyId { get; set; }

        [ForeignKey("FacultyId")]
        public Faculty? Faculty { get; set; }

        public enum ProjectStatus
        {
            Pending,
            InProgress,
            Completed,
            Cancelled
        }

        public ICollection<Document> Documents { get; set; } = new List<Document>();
        public Project GetSafeProject()
        {
            return new Project
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                StartDate = this.StartDate,
                EndDate = this.EndDate,
                Status = this.Status,
                FacultyId = this.FacultyId
            };
        }
    }
}
