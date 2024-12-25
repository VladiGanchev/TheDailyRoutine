using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDailyRoutine.Core.Models.ServiceModels.Habits
{
    public class UserHabitStatusModel
    {
        public int HabitId { get; set; }

        public string Title { get; set; } = string.Empty;

        public int CurrentStreak { get; set; }

        public int BestStreak { get; set; }

        public int TotalCompletions { get; set; }

        public double CompletionRate { get; set; }

        public DateTime LastCompletedAt { get; set; }

    }
}
