namespace TheDailyRoutine.Core.Models.ServiceModels.Statistics
{
    public class HabitProgressSummaryModel
    {
        public int HabitId { get; set; }

        public string Title { get; set; } = string.Empty;

        public int CompletionCount { get; set; }

        public double CompletionRate { get; set; }

        public int CurrentStreak { get; set; }
    }
}
