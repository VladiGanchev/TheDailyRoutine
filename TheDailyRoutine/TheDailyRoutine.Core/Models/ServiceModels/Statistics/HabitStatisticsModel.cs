using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDailyRoutine.Core.Models.ServiceModels.Statistics
{
    public class HabitStatisticsModel
    {
        public int HabitId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int TotalCompletions { get; set; }
        public int TotalAttempts { get; set; }
        public double SuccessRate { get; set; }
        public int CurrentStreak { get; set; }
        public int BestStreak { get; set; }
        //new
        public DateTime? LastCompletedAt { get; set; }
        public int DaysActive { get; set; }
        public double AverageCompletionsPerWeek { get; set; }
        public bool IsActive => LastCompletedAt?.AddDays(7) >= DateTime.UtcNow;

    }

}
