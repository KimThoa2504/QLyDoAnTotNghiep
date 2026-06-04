using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLyDoAnTotNghiep.Models.Reports
{
    public class Report
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [Column("report_type")]
        public string ReportType { get; set; } = string.Empty; 

        public string? Description { get; set; }

        [Column("generated_at")]
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(500)]
        [Column("file_path")]
        public string? FilePath { get; set; }

        public Report GetSafeReport()
        {
            return new Report
            {
                Id = this.Id,
                ReportType = this.ReportType,
                Description = this.Description,
                GeneratedAt = this.GeneratedAt,
                FilePath = this.FilePath
            };
        }
    }
}
