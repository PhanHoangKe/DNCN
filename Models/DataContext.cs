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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure relationship
        modelBuilder.Entity<Users>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
    }
}