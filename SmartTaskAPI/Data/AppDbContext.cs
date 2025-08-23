using Microsoft.EntityFrameworkCore;
using SmartTaskAPI.Models;
using System;

namespace SmartTaskAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskItem>().HasKey(t => t.TaskId);

            
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            base.OnModelCreating(modelBuilder);
        }


        public DbSet<TaskItem> TaskItems => Set<TaskItem>();
        
        public DbSet<User> Users => Set<User>();
        
    }
}
