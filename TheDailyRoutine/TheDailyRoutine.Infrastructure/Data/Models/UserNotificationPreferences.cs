using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDailyRoutine.Infrastructure.Data.Models
{
    public class UserNotificationPreferences
    {
        [Key]
        public string UserId { get; set; } = string.Empty;
        
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        public bool EmailNotificationsEnabled { get; set; } = true;

        public bool DailyReminderEnabled { get; set; } = true;

        public bool WeeklySummaryEnabled { get; set; } = true;

        public bool AchievementNotificationsEnabled { get; set; } = true;

        public bool StreakMilestoneNotificationsEnabled { get; set; } = true;

        public TimeSpan? DailyReminderTime { get; set; }

        public DayOfWeek WeeklySummaryDay { get; set; } = DayOfWeek.Monday;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
