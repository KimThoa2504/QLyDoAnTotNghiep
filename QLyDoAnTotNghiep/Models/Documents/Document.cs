using QLyDoAnTotNghiep.Models.Projects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QLyDoAnTotNghiep.Models.Documents
{
    public class Document
    {
            [Key]
            [Column("id")]
            public long Id { get; set; }

            [Required]
            [JsonIgnore]
            [Column("project_id")]
        public int ProjectId { get; set; }

            [ForeignKey("ProjectId")]
            public Project? Project { get; set; }

            [Required]
            [MaxLength(255)]
            [Column("file_name")]
            public string FileName { get; set; } = string.Empty;

            [Required]
            [MaxLength(500)]
            [Column("file_path")]
            public string FilePath { get; set; } = string.Empty;

            [Column("file_size")]
            public long FileSize { get; set; } = 0;

            [MaxLength(50)]
            [Column("file_type")]
            public string? FileType { get; set; }

        [Column("uploaded_at")]
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // Dùng cho Cloudinary (nếu sau này chuyển sang)
        [Column("public_id")]
            public string? PublicId { get; set; }

            public Document GetSafeDocument()
            {
                return new Document
                {
                    Id = this.Id,
                    ProjectId = this.ProjectId,
                    FileName = this.FileName,
                    FilePath = this.FilePath,
                    FileSize = this.FileSize,
                    FileType = this.FileType,
                    UploadedAt = this.UploadedAt
                };
            }
        }
}
    