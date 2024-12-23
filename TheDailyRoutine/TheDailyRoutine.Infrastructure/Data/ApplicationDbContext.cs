using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Numerics;
using TheDailyRoutine.Infrastructure.Data.Configuration;
using TheDailyRoutine.Infrastructure.Data.Models;

namespace TheDailyRoutine.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Completion>()
                .HasOne(x => x.UserHabit)
                .WithMany(x => x.Completions)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Notification>()
                .HasMany(x => x.UserNotifications)
                .WithOne(x => x.Notification)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Habit>()
                .HasMany(x => x.UsersHabits)
                .WithOne(x => x.Habit)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserHabit>()
                .HasMany(x => x.Completions)
                .WithOne(x => x.UserHabit)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserHabit>()
                .HasOne(x => x.Habit)
                .WithMany(x => x.UsersHabits)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserNotification>()
                .HasOne(x => x.Notification)
                .WithMany(x => x.UserNotifications)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserNotification>()
                .HasKey(x => new { x.UserId, x.NotificationId });

            builder.Entity<UserHabit>()
                .HasKey(x => new { x.UserId, x.HabitId });

            builder.Entity<UserNotification>()
              .HasKey(x => new { x.UserId, x.NotificationId });

            builder.Entity<UserNotification>()
                .Property(x => x.IsRead)
                .HasDefaultValue(false);

            builder.Entity<UserNotification>()
                .Property(x => x.IsEmailed)
                .HasDefaultValue(false);

            builder.Entity<Notification>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Entity<UserNotificationPreferences>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");


            builder.ApplyConfiguration(new HabitConfiguration());

            base.OnModelCreating(builder);
        }

        public DbSet<Completion> Completions { get; set; }
        public DbSet<Habit> Habits { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserHabit> UsersHabits { get; set; }
        public DbSet<UserNotification> UsersNotifications { get; set; }
        public DbSet<UserNotificationPreferences> UserNotificationPreferences { get; set; }


    }
}