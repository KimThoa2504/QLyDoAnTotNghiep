using Microsoft.EntityFrameworkCore;
using QLyDoAnTotNghiep.Models.BoardMembers;
using QLyDoAnTotNghiep.Models.Documents;
using QLyDoAnTotNghiep.Models.EvaluationBoards;
using QLyDoAnTotNghiep.Models.Evaluations;
using QLyDoAnTotNghiep.Models.Faculties;
using QLyDoAnTotNghiep.Models.ProjectMembers;
using QLyDoAnTotNghiep.Models.Projects;
using QLyDoAnTotNghiep.Models.Reports;
using QLyDoAnTotNghiep.Models.Users;

namespace QLyDoAnTotNghiep.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<EvaluationBoard> EvaluationBoards { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<BoardMember> BoardMembers { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<EvaluationCriterion> EvaluationCriteria { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.Property(u => u.PasswordHash).HasColumnName("password");
                entity.Property(u => u.FullName).HasColumnName("full_name");
                entity.Property(u => u.Role).HasColumnName("role");
                entity.Property(u => u.CreatedAt).HasColumnName("created_at");

                entity.Property(u => u.Role)
                      .HasConversion<string>(); 
            });
            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Projects");
                entity.Property(p => p.Status)
                      .HasConversion<string>();        
            });
            modelBuilder.Entity<ProjectMember>(entity =>
            {
                entity.ToTable("ProjectMembers");
                entity.Property(p => p.Role)
                    .HasConversion<string>();
            });
            modelBuilder.Entity<BoardMember>(entity =>
            {
                entity.ToTable("BoardMembers");
                entity.Property(p => p.Role)
                    .HasConversion<string>();
            });

            modelBuilder.Entity<Document>(entity =>
            {
                entity.ToTable("documents");

                entity.HasKey(d => d.Id);

                entity.Property(d => d.Id).HasColumnName("id");
                entity.Property(d => d.ProjectId).HasColumnName("project_id");
                entity.Property(d => d.FileName).HasColumnName("file_name");
                entity.Property(d => d.FilePath).HasColumnName("file_path");
                entity.Property(d => d.FileSize).HasColumnName("file_size");
                entity.Property(d => d.FileType).HasColumnName("file_type");
                entity.Property(d => d.UploadedAt).HasColumnName("uploaded_at");
                entity.Property(d => d.PublicId).HasColumnName("public_id");

                // Foreign Key Configuration - RẤT QUAN TRỌNG
                entity.HasOne(d => d.Project)
                      .WithMany(p => p.Documents)        // Nếu Project có ICollection<Document> Documents
                      .HasForeignKey(d => d.ProjectId)
                      .HasConstraintName("documents_ibfk_1")   // Tên constraint trong DB
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Faculty>().HasIndex(f => f.Name).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<ProjectMember>().HasIndex(pm => new { pm.ProjectId, pm.MaSinhVien }).IsUnique();
            modelBuilder.Entity<BoardMember>().HasIndex(bm => new { bm.BoardId, bm.UserId }).IsUnique();

            modelBuilder.Entity<EvaluationBoard>(entity =>
            {
                entity.ToTable("EvaluationBoards");

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(e => e.Description)
                      .HasColumnType("text");

                entity.Property(e => e.Status)
                      .HasConversion<string>()           
                      .HasMaxLength(20);

                entity.Property(e => e.Type)
                      .HasConversion<string>()
                      .HasMaxLength(20);

                entity.Property(e => e.FormedDate)
                      .HasColumnType("date");

                entity.Property(e => e.ExpiredDate)
                      .HasColumnType("date");

                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                      .HasColumnType("datetime");
            });

            modelBuilder.Entity<Evaluation>(entity =>
            {
                entity.ToTable("Evaluations");

                entity.Property(e => e.Session)
                      .HasConversion<string>()
                      .HasMaxLength(20);

                entity.Property(e => e.Status)
                      .HasConversion<string>()
                      .HasMaxLength(20);

                entity.Property(e => e.Comments)
                      .HasColumnType("text");

                entity.Property(e => e.EvaluationDate)
                      .HasColumnType("date");

                entity.Property(e => e.TotalScore)
                      .HasColumnType("decimal(5,2)");

                // Relationships
                entity.HasOne(e => e.Project)
                      .WithMany()   
                      .HasForeignKey(e => e.ProjectId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.EvaluationBoard)
                        .WithMany(b => b.Evaluations)
                        .HasForeignKey(e => e.BoardId)
                        .HasConstraintName("FK_Evaluations_EvaluationBoards");
                });

            modelBuilder.Entity<EvaluationCriterion>(entity =>
            {
                entity.ToTable("EvaluationCriteria");

                entity.Property(e => e.CriterionName)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(e => e.Score)
                      .HasColumnType("decimal(5,2)");

                entity.Property(e => e.Weight)
                      .HasColumnType("decimal(5,2)");
            });

        }


        protected AppDbContext()
        {
        }
    }
}
