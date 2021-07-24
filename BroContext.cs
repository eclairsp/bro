using System;
using Microsoft.EntityFrameworkCore;

namespace bro
{
    class BroContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Project> Projects { get; set; }

        public string DbPath { get; private set; }

        public BroContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}bro.db";
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>()
                .Property(task => task.Created)
                .HasDefaultValueSql("datetime('now', 'localtime')");

            modelBuilder.Entity<Project>()
                .Property(proj => proj.Created)
                .HasDefaultValueSql("datetime('now', 'localtime')");

            modelBuilder.Entity<Project>()
                .HasIndex(proj => proj.Name)
                .IsUnique();

            modelBuilder.Entity<Project>()
                .HasData(new Project {ProjectID = 1 ,Name = "General" });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}
