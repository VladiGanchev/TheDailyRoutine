using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDailyRoutine.Infrastructure.Data.Models
{
    public class UserNotification
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        public ApplicationUser User { get; set; } = null!;

        [Required]
        public int NotificationId { get; set; }
        public Notification Notification { get; set; } = null!;

    }
}
