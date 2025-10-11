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
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}