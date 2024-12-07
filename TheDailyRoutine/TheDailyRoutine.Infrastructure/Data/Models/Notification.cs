using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TheDailyRoutine.Infrastructure.Data.Enums;
using static TheDailyRoutine.Infrastructure.Data.Constants.DataConstants;

namespace TheDailyRoutine.Infrastructure.Data.Models
{
    public class Notification
    {
        public int Id { get; set; }

        [Required]
        public NotificationType Type { get; set; }

        [Required]
        [MaxLength(NotificationContentMaxLength)]
        public string Content { get; set; } = string.Empty;

        [Required]
        public bool Mailable { get; set; } 

        public ICollection<UserNotification> UserNotifications { get; set; } = new List<UserNotification>();
    }
}
