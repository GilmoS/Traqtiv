using Microsoft.EntityFrameworkCore;
using Traqtiv.API.Models.Entities;

namespace Traqtiv.API.Data
{
    public class SmartFitnessDb : DbContext
    {
        public SmartFitnessDb(DbContextOptions<SmartFitnessDb> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<BodyMetrics> BodyMetrics { get; set; }
        public DbSet<DailyActivity> DailyActivities { get; set; }
        public DbSet<Recommendation> Recommendations { get; set; }
        public DbSet<Alert> Alerts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            // Workout
            modelBuilder.Entity<Workout>().HasOne(w => w.User).WithMany(u => u.Workouts).HasForeignKey(w => w.UserId).OnDelete(DeleteBehavior.Cascade);

            // BodyMetrics
            modelBuilder.Entity<BodyMetrics>().HasOne(b => b.User).WithMany(u => u.BodyMetrics).HasForeignKey(b => b.UserId).OnDelete(DeleteBehavior.Cascade);

            // DailyActivity
            modelBuilder.Entity<DailyActivity>().HasOne(d => d.User).WithMany(u => u.DailyActivities).HasForeignKey(d => d.UserId).OnDelete(DeleteBehavior.Cascade);

            // Recommendation
            modelBuilder.Entity<Recommendation>().HasOne(r => r.User).WithMany(u => u.Recommendations).HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.Cascade);

            // Alert
            modelBuilder.Entity<Alert>().HasOne(a => a.User).WithMany(u => u.Alerts).HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}