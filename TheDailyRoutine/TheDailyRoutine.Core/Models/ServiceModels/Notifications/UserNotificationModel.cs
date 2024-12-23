using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheDailyRoutine.Infrastructure.Data.Enums;
using TheDailyRoutine.Infrastructure.Data.Constants;

namespace TheDailyRoutine.Core.Models.ServiceModels.Notifications
{
    public class UserNotificationModel
    {
        public int Id { get; set; }

        public NotificationType Type { get; set; }

        [Required]
        [StringLength(DataConstants.NotificationContentMaxLength,
            MinimumLength = DataConstants.NotificationContentMinLength,
            ErrorMessage = DataConstants.StringLengthErrorMessage)]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public bool IsRead { get; set; }

        public bool IsEmailed { get; set; }
    }

    internal class NotificationQueueItem
    {
        public string UserId { get; set; }

        public NotificationType Type { get; set; }

        public string Content { get; set; }

        public bool SendEmail { get; set; }

        public DateTime ScheduledFor { get; set; }
    }
}
