using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheDailyRoutine.Core.Models.ServiceModels.Completions;

namespace TheDailyRoutine.Core.Models.ServiceModels.Habits
{
    public class UserHabitDetailsModel
    {
        public int HabitId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Frequency { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CurrentStreak { get; set; }

        public int BestStreak { get; set; }

        public double CompletionRate { get; set; }

        public IEnumerable<CompletionDetailsModel> RecentCompletions { get; set; }
            = new List<CompletionDetailsModel>();
    }
}
