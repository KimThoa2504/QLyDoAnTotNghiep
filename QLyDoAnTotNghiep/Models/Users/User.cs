using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLyDoAnTotNghiep.Models.Users
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [Column("password")]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [Column("full_name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column("role")]
        public UserRole Role { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public enum UserRole
        {
            Admin,
            Lecturer
        }

        // 
        public User GetSafeUser()
        {
            return new User
            {
                Id = this.Id,
                Username = this.Username,
                FullName = this.FullName,
                Email = this.Email,
                Role = this.Role,
                CreatedAt = this.CreatedAt
            };
        }
    }
}

