using System;
using GIS_Upload_Page.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GIS_Upload_Page.Models
{
    public partial class Upload_PageContext : DbContext, IUploadPageContext
    {
        public Upload_PageContext()
        {
        }

        public Upload_PageContext(DbContextOptions<Upload_PageContext> options)
            : base(options)
        {
        }

        public virtual DbSet<File> File { get; set; }
        public virtual DbSet<FileContent> FileContent { get; set; }
        public virtual DbSet<FileProperty> FileProperty { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DTALDBG001;Database=Upload_Page;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<File>(entity =>
            {
                entity.HasIndex(e => e.CreateUserId);

                entity.HasIndex(e => e.ModifyUserId);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateUserId).HasColumnName("Create_User_ID");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnName("Created_DateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.ModifiedDateTime)
                    .HasColumnName("Modified_DateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifyUserId).HasColumnName("Modify_User_ID");

                entity.HasOne(d => d.CreateUser)
                    .WithMany(p => p.FileCreateUser)
                    .HasForeignKey(d => d.CreateUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_File_Create_User");

                entity.HasOne(d => d.ModifyUser)
                    .WithMany(p => p.FileModifyUser)
                    .HasForeignKey(d => d.ModifyUserId)
                    .HasConstraintName("FK_File_Modify_User");
            });

            modelBuilder.Entity<FileContent>(entity =>
            {
                entity.HasIndex(e => e.FileId);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Content).IsRequired();

                entity.Property(e => e.FileId).HasColumnName("File_ID");

                entity.HasOne(d => d.File)
                    .WithMany(p => p.FileContent)
                    .HasForeignKey(d => d.FileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FileContent_File");
            });

            modelBuilder.Entity<FileProperty>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Comment).HasColumnType("text");

                entity.Property(e => e.FileId).HasColumnName("File_ID");

                entity.HasOne(d => d.File)
                    .WithMany(p => p.FileProperty)
                    .HasForeignKey(d => d.FileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FileProperty_File");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.FirstName)
                    .HasColumnName("First_Name")
                    .HasMaxLength(255);

                entity.Property(e => e.FullName)
                    .HasColumnName("Full_Name")
                    .HasMaxLength(255);

                entity.Property(e => e.LastName)
                    .HasColumnName("Last_Name")
                    .HasMaxLength(255);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("User_Name")
                    .HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
