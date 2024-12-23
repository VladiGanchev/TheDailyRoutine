using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TheDailyRoutine.Core.Contracts;
using TheDailyRoutine.Core.Models.ServiceModels.Notifications;
using TheDailyRoutine.Infrastructure.Data;
using TheDailyRoutine.Infrastructure.Data.Enums;
using TheDailyRoutine.Infrastructure.Data.Models;

namespace TheDailyRoutine.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            ApplicationDbContext context,
            IEmailService emailService,
            ILogger<NotificationService> logger)
        {
            _context = context;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<(bool success, string error)> CreateReminderNotificationAsync(
            string userId,
            IEnumerable<int> habitIds,
            bool sendEmail = true)
        {
            try
            {
                var habits = await _context.Habits
                    .Where(h => habitIds.Contains(h.Id))
                    .ToListAsync();

                var content = $"Remember to complete your habits: {string.Join(", ", habits.Select(h => h.Title))}";

                var notification = new Notification
                {
                    Type = NotificationType.Reminder,
                    Content = content,
                    Mailable = sendEmail,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Notifications.AddAsync(notification);

                var userNotification = new UserNotification
                {
                    UserId = userId,
                    NotificationId = notification.Id
                };

                await _context.UsersNotifications.AddAsync(userNotification);
                await _context.SaveChangesAsync();

                if (sendEmail)
                {
                    var user = await _context.Users.FindAsync(userId);
                    if (user != null)
                    {
                        await _emailService.SendNotificationEmailAsync(
                            user.Email,
                            "Daily Habit Reminder",
                            content);
                    }
                }

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating reminder notification for user {UserId}", userId);
                return (false, "Failed to create reminder notification");
            }
        }

        public async Task<(bool success, string error)> CreateWeeklySummaryNotificationAsync(
            string userId,
            DateTime weekStartDate,
            bool sendEmail = true)
        {
            try
            {
                var userHabits = await _context.UsersHabits
                    .Include(uh => uh.Habit)
                    .Include(uh => uh.Completions)
                    .Where(uh => uh.UserId == userId)
                    .ToListAsync();

                var weekEndDate = weekStartDate.AddDays(7);
                var totalHabits = userHabits.Count;
                var completedHabits = userHabits.Count(h =>
                    h.Completions.Any(c =>
                        c.CompletedAt >= weekStartDate &&
                        c.CompletedAt < weekEndDate &&
                        c.Completed));

                var completionRate = totalHabits > 0
                    ? (double)completedHabits / totalHabits * 100
                    : 0;

                var content = $"Weekly Summary:\n" +
                            $"Completion Rate: {completionRate:F1}%\n" +
                            $"Habits Completed: {completedHabits}/{totalHabits}";

                var notification = new Notification
                {
                    Type = NotificationType.Achievement,
                    Content = content,
                    Mailable = sendEmail,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Notifications.AddAsync(notification);

                var userNotification = new UserNotification
                {
                    UserId = userId,
                    NotificationId = notification.Id
                };

                await _context.UsersNotifications.AddAsync(userNotification);
                await _context.SaveChangesAsync();

                if (sendEmail)
                {
                    var user = await _context.Users.FindAsync(userId);
                    if (user != null)
                    {
                        await _emailService.SendNotificationEmailAsync(
                            user.Email,
                            "Weekly Habit Summary",
                            content);
                    }
                }

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating weekly summary for user {UserId}", userId);
                return (false, "Failed to create weekly summary");
            }
        }

        public async Task<(bool success, string error)> CreateStreakMilestoneNotificationAsync(
            string userId,
            int habitId,
            int streakCount,
            bool sendEmail = true)
        {
            try
            {
                var habit = await _context.Habits.FindAsync(habitId);
                if (habit == null)
                {
                    return (false, "Habit not found");
                }

                var content = $"Congratulations! You've maintained a {streakCount}-day streak for {habit.Title}!";

                var notification = new Notification
                {
                    Type = NotificationType.Achievement,
                    Content = content,
                    Mailable = sendEmail,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Notifications.AddAsync(notification);

                var userNotification = new UserNotification
                {
                    UserId = userId,
                    NotificationId = notification.Id
                };

                await _context.UsersNotifications.AddAsync(userNotification);
                await _context.SaveChangesAsync();

                if (sendEmail)
                {
                    var user = await _context.Users.FindAsync(userId);
                    if (user != null)
                    {
                        await _emailService.SendNotificationEmailAsync(
                            user.Email,
                            "Streak Achievement!",
                            content);
                    }
                }

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating streak milestone notification for user {UserId}", userId);
                return (false, "Failed to create streak milestone notification");
            }
        }

        public async Task<(bool success, string error)> CreateAchievementNotificationAsync(
                    string userId,
                    string achievement,
                    bool sendEmail = true)
        {
            try
            {
                var notification = new Notification
                {
                    Type = NotificationType.Achievement,
                    Content = achievement,
                    Mailable = sendEmail,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Notifications.AddAsync(notification);

                var userNotification = new UserNotification
                {
                    UserId = userId,
                    NotificationId = notification.Id
                };

                await _context.UsersNotifications.AddAsync(userNotification);
                await _context.SaveChangesAsync();

                if (sendEmail)
                {
                    var user = await _context.Users.FindAsync(userId);
                    if (user != null)
                    {
                        await _emailService.SendNotificationEmailAsync(
                            user.Email,
                            "Achievement Unlocked!",
                            achievement);
                    }
                }

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating achievement notification for user {UserId}", userId);
                return (false, "Failed to create achievement notification");
            }
        }

        public async Task<IEnumerable<UserNotificationModel>> GetUnreadNotificationsAsync(string userId)
        {
            return await _context.UsersNotifications
                .Include(un => un.Notification)
                .Where(un => un.UserId == userId && !un.IsRead)
                .OrderByDescending(un => un.Notification.CreatedAt)
                .Select(un => new UserNotificationModel
                {
                    Id = un.NotificationId,
                    Type = un.Notification.Type,
                    Content = un.Notification.Content,
                    CreatedAt = un.Notification.CreatedAt,
                    IsRead = un.IsRead,
                    IsEmailed = un.IsEmailed
                })
                .ToListAsync();
        }

        public async Task<bool> MarkNotificationAsReadAsync(string userId, int notificationId)
        {
            try
            {
                var userNotification = await _context.UsersNotifications
                    .FirstOrDefaultAsync(un =>
                        un.UserId == userId &&
                        un.NotificationId == notificationId);

                if (userNotification == null)
                {
                    return false;
                }

                userNotification.IsRead = true;
                userNotification.ReadAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking notification as read for user {UserId}", userId);
                return false;
            }
        }

        public async Task ProcessPendingNotificationsAsync()
        {
            try
            {
                var pendingNotifications = await _context.UsersNotifications
                    .Include(un => un.Notification)
                    .Include(un => un.User)
                    .Where(un =>
                        un.Notification.Mailable &&
                        !un.IsEmailed)
                    .ToListAsync();

                foreach (var notification in pendingNotifications)
                {
                    if (notification.User.Email != null)
                    {
                        var emailResult = await _emailService.SendNotificationEmailAsync(
                            notification.User.Email,
                            GetNotificationSubject(notification.Notification.Type),
                            notification.Notification.Content);

                        if (emailResult)
                        {
                            notification.IsEmailed = true;
                            notification.EmailedAt = DateTime.UtcNow;
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing pending notifications");
                throw;
            }
        }

        /* public async Task<UserNotificationPreferencesModel> GetUserNotificationPreferencesAsync(string userId)
         {
             var preferences = await _context.UserNotificationPreferences
                 .FirstOrDefaultAsync(p => p.UserId == userId);

             if (preferences == null)
             {
                 return new UserNotificationPreferencesModel
                 {
                     EmailNotificationsEnabled = true,
                     DailyReminderEnabled = true,
                     WeeklySummaryEnabled = true,
                     AchievementNotificationsEnabled = true,
                     StreakMilestoneNotificationsEnabled = true,
                     DailyReminderTime = new TimeSpan(20, 0, 0), // 8:00 PM
                     WeeklySummaryDay = DayOfWeek.Monday,
                     NotificationTypePreferences = new Dictionary<NotificationType, bool>
                     {
                         { NotificationType.Reminder, true },
                         { NotificationType.Achievement, true },
                         { NotificationType.Greeting, true }
                     }
                 };
             }

             return new UserNotificationPreferencesModel
             {
                 EmailNotificationsEnabled = preferences.EmailNotificationsEnabled,
                 DailyReminderEnabled = preferences.DailyReminderEnabled,
                 WeeklySummaryEnabled = preferences.WeeklySummaryEnabled,
                 AchievementNotificationsEnabled = preferences.AchievementNotificationsEnabled,
                 StreakMilestoneNotificationsEnabled = preferences.StreakMilestoneNotificationsEnabled,
                 DailyReminderTime = preferences.DailyReminderTime,
                 WeeklySummaryDay = preferences.WeeklySummaryDay
             };
         }
 */
        public async Task<IEnumerable<UserNotificationModel>> GetNotificationHistoryAsync(
            string userId,
            DateTime startDate,
            DateTime endDate,
            NotificationType? type = null)
        {
            var query = _context.UsersNotifications
                .Include(un => un.Notification)
                .Where(un => un.UserId == userId)
                .Where(un => un.Notification.CreatedAt >= startDate &&
                            un.Notification.CreatedAt <= endDate);

            if (type.HasValue)
            {
                query = query.Where(un => un.Notification.Type == type.Value);
            }

            return await query
                .OrderByDescending(un => un.Notification.CreatedAt)
                .Select(un => new UserNotificationModel
                {
                    Id = un.NotificationId,
                    Type = un.Notification.Type,
                    Content = un.Notification.Content,
                    CreatedAt = un.Notification.CreatedAt,
                    IsRead = un.IsRead,
                    IsEmailed = un.IsEmailed
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateUserNotificationPreferencesAsync(
    string userId,
    UserNotificationPreferencesModel preferences)
        {
            try
            {
                var userPreferences = await _context.UserNotificationPreferences
                    .FirstOrDefaultAsync(p => p.UserId == userId);

                if (userPreferences == null)
                {
                    userPreferences = new UserNotificationPreferences
                    {
                        UserId = userId,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _context.UserNotificationPreferences.AddAsync(userPreferences);
                }

                // Update preferences
                userPreferences.EmailNotificationsEnabled = preferences.EmailNotificationsEnabled;
                userPreferences.DailyReminderEnabled = preferences.DailyReminderEnabled;
                userPreferences.WeeklySummaryEnabled = preferences.WeeklySummaryEnabled;
                userPreferences.AchievementNotificationsEnabled = preferences.AchievementNotificationsEnabled;
                userPreferences.StreakMilestoneNotificationsEnabled = preferences.StreakMilestoneNotificationsEnabled;
                userPreferences.DailyReminderTime = preferences.DailyReminderTime;
                userPreferences.WeeklySummaryDay = preferences.WeeklySummaryDay;
                userPreferences.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Log the update
                _logger.LogInformation(
                    "Updated notification preferences for user {UserId}. Email notifications: {EmailEnabled}",
                    userId,
                    preferences.EmailNotificationsEnabled);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating notification preferences for user {UserId}", userId);
                return false;
            }
        }

        public async Task<UserNotificationPreferencesModel> GetUserNotificationPreferencesAsync(string userId)
        {
            try
            {
                var preferences = await _context.UserNotificationPreferences
                    .FirstOrDefaultAsync(p => p.UserId == userId);

                if (preferences == null)
                {
                    // Return default preferences
                    return new UserNotificationPreferencesModel
                    {
                        EmailNotificationsEnabled = true,
                        DailyReminderEnabled = true,
                        WeeklySummaryEnabled = true,
                        AchievementNotificationsEnabled = true,
                        StreakMilestoneNotificationsEnabled = true,
                        DailyReminderTime = new TimeSpan(20, 0, 0), // 8:00 PM
                        WeeklySummaryDay = DayOfWeek.Monday,
                        NotificationTypePreferences = new Dictionary<NotificationType, bool>
                {
                    { NotificationType.Reminder, true },
                    { NotificationType.Achievement, true },
                    { NotificationType.Greeting, true }
                }
                    };
                }

                return new UserNotificationPreferencesModel
                {
                    EmailNotificationsEnabled = preferences.EmailNotificationsEnabled,
                    DailyReminderEnabled = preferences.DailyReminderEnabled,
                    WeeklySummaryEnabled = preferences.WeeklySummaryEnabled,
                    AchievementNotificationsEnabled = preferences.AchievementNotificationsEnabled,
                    StreakMilestoneNotificationsEnabled = preferences.StreakMilestoneNotificationsEnabled,
                    DailyReminderTime = preferences.DailyReminderTime,
                    WeeklySummaryDay = preferences.WeeklySummaryDay,
                    NotificationTypePreferences = new Dictionary<NotificationType, bool>
            {
                { NotificationType.Reminder, preferences.DailyReminderEnabled },
                { NotificationType.Achievement, preferences.AchievementNotificationsEnabled },
                { NotificationType.Greeting, true }
            }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving notification preferences for user {UserId}", userId);
                throw;
            }
        }

        #region Helper Methods

        private string GetNotificationSubject(NotificationType type)
        {
            return type switch
            {
                NotificationType.Reminder => "Daily Habit Reminder",
                NotificationType.Achievement => "Achievement Unlocked!",
                NotificationType.Greeting => "Welcome to Daily Routine",
                _ => "Notification from Daily Routine"
            };
        }

        #endregion
    }
}