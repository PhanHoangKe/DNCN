using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduFlex.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;

namespace EduFlex.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = true;
        }
        public DbSet<AdminMenu> AdminMenus { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Categories> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // Quan hệ giữa User và Role
    modelBuilder.Entity<Users>()
        .HasOne(u => u.Role)
        .WithMany(r => r.Users)
        .HasForeignKey(u => u.RoleId)
        .OnDelete(DeleteBehavior.Restrict);

    // ✅ Quan hệ giữa Course và ApprovedBy (User duyệt khóa học)
    modelBuilder.Entity<Course>()
        .HasOne(c => c.ApprovedByNavigation)
        .WithMany()
        .HasForeignKey(c => c.ApprovedBy)
        .OnDelete(DeleteBehavior.Restrict);

    // ✅ Quan hệ giữa Course và Instructor (giảng viên)
    modelBuilder.Entity<Course>()
        .HasOne(c => c.Instructor)
        .WithMany()
        .HasForeignKey(c => c.InstructorId)
        .OnDelete(DeleteBehavior.Restrict);

    // ✅ Quan hệ giữa Course và Category
    modelBuilder.Entity<Course>()
        .HasOne(c => c.Categories)
        .WithMany(cat => cat.Courses)
        .HasForeignKey(c => c.CategoryId)
        .OnDelete(DeleteBehavior.Restrict);

    // ✅ Quan hệ giữa Course và Level
    modelBuilder.Entity<Course>()
        .HasOne(c => c.Level)
        .WithMany()
        .HasForeignKey(c => c.LevelId)
        .OnDelete(DeleteBehavior.Restrict);
}
    }
}