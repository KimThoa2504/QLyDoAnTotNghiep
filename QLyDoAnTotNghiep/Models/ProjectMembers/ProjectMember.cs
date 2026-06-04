using QLyDoAnTotNghiep.Models.Projects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QLyDoAnTotNghiep.Models.ProjectMembers
{
    public class ProjectMember
    {
        [Key]
        public int Id { get; set; }

        [Column("project_id")]
        public int ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public Project? Project { get; set; }

        [Column("masinhvien")]
        [MaxLength(50)]
        public string? MaSinhVien { get; set; }

        [Column("hovaten")]
        [MaxLength(255)]
        public string? HoVaTen { get; set; }

        [Column("role")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MemberRole Role { get; set; } = MemberRole.Member;

        public enum MemberRole
        {
            Leader,
            Member,
            Supporter
        }

        public ProjectMember GetSafeProjectMember()
        {
            return new ProjectMember
            {
                Id = this.Id,
                ProjectId = this.ProjectId,
                MaSinhVien = this.MaSinhVien,
                HoVaTen = this.HoVaTen,
                Role = this.Role
            };
        }
    }
}
