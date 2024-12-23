using Microsoft.Extensions.DependencyInjection;
using TheDailyRoutine.Core.Contracts;
using TheDailyRoutine.Core.Services;

namespace TheDailyRoutine.Core.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            // Register Core Services
            services.AddScoped<IHabitService, HabitService>();
            services.AddScoped<IUserHabitService, UserHabitService>();
            services.AddScoped<ICompletionService, CompletionService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IStatisticsService, StatisticsService>();

            // Register Background Job Services
            services.AddScoped<NotificationJobService>();

            return services;
        }
    }
}