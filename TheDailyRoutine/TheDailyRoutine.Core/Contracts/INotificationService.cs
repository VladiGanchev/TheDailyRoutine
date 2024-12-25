using TheDailyRoutine.Core.Models.ServiceModels.Notifications;
using TheDailyRoutine.Infrastructure.Data.Enums;

namespace TheDailyRoutine.Core.Contracts
{
    public interface INotificationService
    {
        Task<(bool success, string error)> CreateReminderNotificationAsync(
            string userId,
            IEnumerable<int> habitIds,
            bool sendEmail = true);

        Task<(bool success, string error)> CreateWeeklySummaryNotificationAsync(
            string userId,
            DateTime weekStartDate,
            bool sendEmail = true);

        Task<(bool success, string error)> CreateStreakMilestoneNotificationAsync(
            string userId,
            int habitId,
            int streakCount,
            bool sendEmail = true);

        Task<(bool success, string error)> CreateAchievementNotificationAsync(
            string userId,
            string achievement,
            bool sendEmail = true);

        Task<IEnumerable<UserNotificationModel>> GetUnreadNotificationsAsync(
            string userId);

        Task<bool> MarkNotificationAsReadAsync(
            string userId,
            int notificationId);

        Task ProcessPendingNotificationsAsync();

        Task<UserNotificationPreferencesModel> GetUserNotificationPreferencesAsync(
            string userId);

        Task<IEnumerable<UserNotificationModel>> GetNotificationHistoryAsync(
            string userId,
            DateTime startDate,
            DateTime endDate,
            NotificationType? type = null);
   
     
        /// <summary>
        /// Updates user notification preferences
        /// </summary>
        /// <returns>True if update was successful, false otherwise</returns>
        Task<bool> UpdateUserNotificationPreferencesAsync(string userId, UserNotificationPreferencesModel preferences);

    }
}