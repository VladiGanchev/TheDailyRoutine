using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TheDailyRoutine.Core.Services;

namespace TheDailyRoutine.Infrastructure.Extensions
{
    public static class HangfireExtensions
    {
        public static IServiceCollection AddHangfireServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Add Hangfire services
            services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"),
                    new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.FromSeconds(10),
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true
                    }));

            // Add the Hangfire server
            services.AddHangfireServer(options =>
            {
                options.WorkerCount = Environment.ProcessorCount * 2;
                options.Queues = new[] { "notifications", "default" };
            });

            return services;
        }

        public static IApplicationBuilder UseHangfireDashboard(
            this IApplicationBuilder app,
            IConfiguration configuration)
        {
            var options = new DashboardOptions
            {
                DashboardTitle = "Daily Routine Jobs",
                IsReadOnlyFunc = (context) => false
            };

            app.UseHangfireDashboard("/jobs", options);

            // Schedule recurring jobs
            RecurringJob.AddOrUpdate<NotificationJobService>(
                "daily-reminders",
                x => x.SendDailyReminders(),
                "0 20 * * *"); // Every day at 8 PM

            RecurringJob.AddOrUpdate<NotificationJobService>(
                "weekly-summaries",
                x => x.SendWeeklySummaries(),
                "0 18 * * 0"); // Every Sunday at 6 PM

            RecurringJob.AddOrUpdate<NotificationJobService>(
                "process-notifications",
                x => x.ProcessPendingNotifications(),
                "*/5 * * * *"); // Every 5 minutes

            RecurringJob.AddOrUpdate<NotificationJobService>(
                "streak-milestones",
                x => x.CheckAndSendStreakMilestones(),
                "0 0 * * *"); // Every day at midnight

            return app;
        }
    }
}
