using TheDailyRoutine.Infrastructure.Data.Enums;

namespace TheDailyRoutine.Core.Models.ServiceModels.Notifications
{
    public class UserNotificationPreferencesModel
    {
        public bool EmailNotificationsEnabled { get; set; } = true;

        public bool DailyReminderEnabled { get; set; } = true;

        public bool WeeklySummaryEnabled { get; set; } = true;

        public bool AchievementNotificationsEnabled { get; set; } = true;

        public bool StreakMilestoneNotificationsEnabled { get; set; } = true;

        public TimeSpan? DailyReminderTime { get; set; }

        public DayOfWeek WeeklySummaryDay { get; set; } = DayOfWeek.Monday;

        public Dictionary<NotificationType, bool> NotificationTypePreferences { get; set; } =
            new Dictionary<NotificationType, bool>();
    }
}
