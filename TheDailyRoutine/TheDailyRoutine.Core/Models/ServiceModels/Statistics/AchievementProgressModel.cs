using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDailyRoutine.Core.Models.ServiceModels.Statistics
{
    public class AchievementProgressModel
    {
        public string AchievementId { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public double ProgressPercentage { get; set; }
        public bool IsAchieved { get; set; }
        // Added properties
        public DateTime? AchievedAt { get; set; }
        public int RemainingToComplete { get; set; }
        public DateTime? EstimatedCompletionDate { get; set; }

    }
}
