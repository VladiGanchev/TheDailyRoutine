using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheDailyRoutine.Core.Models.ServiceModels.Notifications;

namespace TheDailyRoutine.Core.Contracts
{
    public interface IEmailService
    {
        /// <summary>
        /// Sends a notification email to a user
        /// </summary>
        Task<bool> SendNotificationEmailAsync(string userEmail, string subject, string content);

        /// <summary>
        /// Sends a weekly summary email to a user
        /// </summary>
        Task<bool> SendWeeklySummaryEmailAsync(string userEmail, WeeklySummaryNotificationModel summary);

        /// <summary>
        /// Sends a streak milestone celebration email
        /// </summary>
        Task<bool> SendStreakMilestoneEmailAsync(string userEmail, string habitTitle, int streakCount);

        /// <summary>
        /// Sends a reminder email for incomplete habits
        /// </summary>
        Task<bool> SendReminderEmailAsync(string userEmail, IEnumerable<string> incompleteHabits);
    }
}
