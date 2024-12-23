using TheDailyRoutine.Core.Models.ServiceModels.Statistics;

namespace TheDailyRoutine.Core.Models.ServiceModels.Notifications
{
    public class WeeklySummaryNotificationModel
    {
        public int TotalHabits { get; set; }

        public int CompletedHabits { get; set; }

        public double CompletionRate { get; set; }

        public int TotalStreaks { get; set; }

        public IEnumerable<HabitProgressSummaryModel> TopHabits { get; set; } =
            new List<HabitProgressSummaryModel>();

        public IEnumerable<HabitProgressSummaryModel> NeedsAttentionHabits { get; set; } =
            new List<HabitProgressSummaryModel>();
    }
}
