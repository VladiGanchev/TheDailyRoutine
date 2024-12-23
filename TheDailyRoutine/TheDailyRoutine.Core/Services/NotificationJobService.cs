using Microsoft.Extensions.Logging;
using TheDailyRoutine.Core.Contracts;

namespace TheDailyRoutine.Core.Services
{
    public class NotificationJobService
    {
        private readonly INotificationService _notificationService;
        private readonly IUserHabitService _userHabitService;
        private readonly ILogger<NotificationJobService> _logger;

        public NotificationJobService(
            INotificationService notificationService,
            IUserHabitService userHabitService,
            ILogger<NotificationJobService> logger)
        {
            _notificationService = notificationService;
            _userHabitService = userHabitService;
            _logger = logger;
        }

        public async Task SendDailyReminders()
        {
            try
            {
                _logger.LogInformation("Starting daily reminders job");

                var usersWithIncompleteHabits = await _userHabitService
                    .GetHabitsNeedingAttentionAsync(DateTime.UtcNow);

                foreach (var userHabits in usersWithIncompleteHabits)
                {
                    var habitIds = userHabits.Value.Select(h => h.HabitId).ToList();
                    await _notificationService.CreateReminderNotificationAsync(
                        userHabits.Key,
                        habitIds);
                }

                _logger.LogInformation("Completed daily reminders job");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in daily reminders job");
                throw;
            }
        }

        public async Task SendWeeklySummaries()
        {
            try
            {
                _logger.LogInformation("Starting weekly summaries job");

                var users = await _userHabitService.GetActiveUsersAsync();
                var weekStart = DateTime.UtcNow.AddDays(-7);

                foreach (var userId in users)
                {
                    await _notificationService.CreateWeeklySummaryNotificationAsync(
                        userId,
                        weekStart);
                }

                _logger.LogInformation("Completed weekly summaries job");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in weekly summaries job");
                throw;
            }
        }

        public async Task CheckAndSendStreakMilestones()
        {
            try
            {
                _logger.LogInformation("Starting streak milestone check job");

                var users = await _userHabitService.GetActiveUsersAsync();
                var milestones = new[] { 7, 30, 60, 90, 180, 365 };

                foreach (var userId in users)
                {
                    var userStreaks = await _userHabitService.GetCurrentStreaksAsync(userId);

                    foreach (var streak in userStreaks)
                    {
                        var milestone = milestones.FirstOrDefault(m => streak.Value == m);
                        if (milestone > 0)
                        {
                            await _notificationService.CreateStreakMilestoneNotificationAsync(
                                userId,
                                streak.Key,
                                milestone);
                        }
                    }
                }

                _logger.LogInformation("Completed streak milestone check job");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in streak milestone check job");
                throw;
            }
        }

        public async Task ProcessPendingNotifications()
        {
            try
            {
                _logger.LogInformation("Starting notification processing job");
                await _notificationService.ProcessPendingNotificationsAsync();
                _logger.LogInformation("Completed notification processing job");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in notification processing job");
                throw;
            }
        }
    }
}